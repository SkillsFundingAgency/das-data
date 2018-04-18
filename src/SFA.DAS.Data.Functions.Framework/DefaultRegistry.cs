//using MediatR;
//using Microsoft.Azure;
//using SFA.DAS.Configuration;
//using SFA.DAS.Configuration.AzureTableStorage;
//using SFA.DAS.Data.Application.Configuration;
//using SFA.DAS.Data.Application.Interfaces.Repositories;
//using SFA.DAS.Data.Infrastructure.Data;
//using SFA.DAS.NLog.Logger;
//using StructureMap;

//namespace SFA.DAS.Data.Functions.Framework
//{
//    public class DefaultRegistry : Registry
//    {
//        private string ServiceName = CloudConfigurationManager.GetSetting("ServiceName");
//        private const string Version = "1.0";

//        public DefaultRegistry()
//        {
//            Scan(scan =>
//            {
//                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
//                scan.RegisterConcreteTypesAgainstTheFirstInterface();
//            });

//            var config = GetConfiguration();

//            For<IDataConfiguration>().Use(config);
//            RegisterRepositories(config.DatabaseConnectionString);
//            AddMediatrRegistrations();

//            ConfigureLogging();
//        }


//        private void RegisterRepositories(string connectionString)
//        {
//            For<IStatisticsRepository>().Use<StatisticsRepository>().Ctor<string>().Is(connectionString);
//        }

//        private void AddMediatrRegistrations()
//        {
//            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
//            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

//            For<IMediator>().Use<Mediator>();
//        }

//        private DataConfiguration GetConfiguration()
//        {
//            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

//            var configurationRepository = GetConfigurationRepository();
//            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, Version));

//            return configurationService.Get<DataConfiguration>();
//        }

//        private static IConfigurationRepository GetConfigurationRepository()
//        {
//            return new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
//        }

//        private void ConfigureLogging()
//        {
//            For<ILog>().Use(x => new NLogLogger(x.ParentType, null, null)).AlwaysUnique();
//        }
//    }
//}
