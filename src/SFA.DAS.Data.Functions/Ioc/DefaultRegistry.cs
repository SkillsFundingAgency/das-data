using System.Linq;
using System.Net.Http;
using MediatR;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Handlers;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Statistics.Services;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Http;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Data.Functions.Ioc
{
    public class DefaultRegistry : Registry
    {
        private string ServiceName = CloudConfigurationManager.GetSetting("ServiceName");
        private const string Version = "1.0";

        public DefaultRegistry()
        {
            Scan(scan =>
            {
                var assemblyNames = (typeof(DefaultRegistry).Assembly.GetReferencedAssemblies()).ToList().Where(w => w.FullName.StartsWith("SFA.DAS.")).Select(a => a.FullName);

                foreach (var assemblyName in assemblyNames)
                {
                    scan.Assembly(assemblyName);
                }

                //scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            var config = GetConfiguration();
            
            For<IDataConfiguration>().Use(config);
            RegisterRepositories(config);
            RegisterApis(config);
            AddMediatrRegistrations();

            ConfigureLogging();
        }


        private void RegisterRepositories(DataConfiguration config)
        {
            // Add registrations here
            For<IStatisticsRepository>().Use<StatisticsRepository>().Ctor<string>().Is(config.DatabaseConnectionString);

            HttpMessageHandler handler = new HttpClientHandler();
            For<IHttpClientWrapper>().Use<HttpClientWrapper>().Ctor<HttpMessageHandler>().Is(handler);
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IAccountApiClient>().Use<AccountApiClient>().Ctor<IAccountApiConfiguration>().Is(config.AccountsApi);
        }

        private DataConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

            var configurationRepository = GetConfigurationRepository();
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, Version));

            return configurationService.Get<DataConfiguration>();
        }

        private static IConfigurationRepository GetConfigurationRepository()
        {
            return new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
        }

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null)).AlwaysUnique();
        }

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }
    }

}
