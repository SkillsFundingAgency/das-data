using System;
using System.Configuration;
using System.Linq;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Configuration.FileStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Infrastructure.Attributes;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.Data.Infrastructure.DependencyResolution
{
    public abstract class MessageServiceBusPolicyBase<T> : ConfiguredInstancePolicy where T : IServiceBusConfiguration
    {
        protected readonly string ServiceName;

        protected MessageServiceBusPolicyBase(string serviceName)
        {
            ServiceName = serviceName;
        }

        protected static string GetConnectionStringName(IConfiguredInstance instance)
        {
            var attribute =
                instance.PluggedType.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(ServiceBusConnectionStringAttribute));

            var argument = attribute?.ConstructorArguments.FirstOrDefault();

            return argument.HasValue ? argument.Value.Value as string : string.Empty;
        }

        protected string GetMessageQueueConnectionString(string environment, string connectionStringName)
        {
            var configurationService = new ConfigurationService(GetConfigurationRepository(),
                new ConfigurationOptions(ServiceName, environment, "1.0"));

            var config = configurationService.Get<T>();

            if (string.IsNullOrEmpty(connectionStringName))
            {
                return config.MessageServiceBusConnectionString;
            }

            if (!config.MessageServiceBusConnectionStringLookup.ContainsKey(connectionStringName))
            {
                throw new InvalidOperationException($"Cannot find service bus connection named {connectionStringName} in configuration.");
            }

            var messageQueueConnectionString = config.MessageServiceBusConnectionStringLookup[connectionStringName];
            return messageQueueConnectionString;
        }

        protected static IConfigurationRepository GetConfigurationRepository()
        {
            IConfigurationRepository configurationRepository;
            if (bool.Parse(ConfigurationManager.AppSettings["LocalConfig"]))
            {
                configurationRepository = new FileStorageConfigurationRepository();
            }
            else
            {
                configurationRepository = new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
            }
            return configurationRepository;
        }

        protected static string GetEnvironmentName()
        {
            var environment = Environment.GetEnvironmentVariable("DASENV");
            if (string.IsNullOrEmpty(environment))
            {
                environment = CloudConfigurationManager.GetSetting("EnvironmentName");
            }
            return environment;
        }
    }
}
