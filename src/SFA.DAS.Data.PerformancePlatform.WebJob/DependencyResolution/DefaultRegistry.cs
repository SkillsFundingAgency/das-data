using MediatR;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Data.PerformancePlatform.WebJob.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        private string ServiceName = CloudConfigurationManager.GetSetting("ServiceName");
        private const string Version = "1.0";

        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            var config = GetConfiguration();

            For<IDataConfiguration>().Use(config);
            RegisterRepositories(config.DatabaseConnectionString);
            AddPerformancePlatformDataExtractors();
            AddMediatrRegistrations();

            ConfigureLogging();
        }
        
        private void RegisterRepositories(string connectionString)
        {
            For<IAccountRepository>().Use<AccountRepository>().Ctor<string>().Is(connectionString);
            For<IPayeSchemeRepository>().Use<PayeSchemeRepository>().Ctor<string>().Is(connectionString);
            For<ILegalEntityRepository>().Use<LegalEntityRepository>().Ctor<string>().Is(connectionString);
            For<IApprenticeshipRepository>().Use<ApprenticeshipRepository>().Ctor<string>().Is(connectionString);
            For<IPerformancePlatformRepository>().Use<PerformancePlatformRepository>().Ctor<string>().Is(connectionString);
        }

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }

        private DataConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

            var configurationRepository = GetConfigurationRepository();
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, Version));

            return configurationService.Get<DataConfiguration>();
        }

        private void AddPerformancePlatformDataExtractors()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.AssemblyContainingType<IPerformancePlatformDataExtractor>();
                scan.AddAllTypesOf<IPerformancePlatformDataExtractor>();
            });
        }

        private static IConfigurationRepository GetConfigurationRepository()
        {
            return new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
        }

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null,null)).AlwaysUnique();
        }
    }
}