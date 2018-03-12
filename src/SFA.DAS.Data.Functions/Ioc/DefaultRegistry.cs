using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using System.Configuration;
using System.Reflection;
using MediatR;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using StructureMap.TypeRules;

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
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            //var config = GetConfiguration();

            //For<IDataConfiguration>().Use(config);
            //RegisterRepositories(config.DatabaseConnectionString);
            //RegisterApis(config);

            //RegisterEventCollectors();
            //RegisterEventHandlers();
            //RegisterEventProcessors();

            //RegisterMapper();

            AddMediatrRegistrations();

            ConfigureLogging();
        }

        //private void RegisterEventHandlers()
        //{
        //    For<IEventHandler<GenericEvent<AccountCreatedEvent>>>().Use<AccountCreatedEventHandler>();
        //    For<IEventHandler<GenericEvent<AccountRenamedEvent>>>().Use<AccountRenamedEventHandler>();
        //    For<IEventHandler<ApprenticeshipEventView>>().Use<ApprenticeshipEventHandler>();
        //    For<IEventHandler<GenericEvent<LegalEntityCreatedEvent>>>().Use<LegalEntityCreatedEventHandler>();
        //    For<IEventHandler<GenericEvent<PayeSchemeAddedEvent>>>().Use<PayeSchemeAddedEventHandler>();
        //    For<IEventHandler<GenericEvent<PayeSchemeRemovedEvent>>>().Use<PayeSchemeRemovedEventHandler>();
        //    For<IEventHandler<GenericEvent<LevyDeclarationUpdatedEvent>>>().Use<LevyDeclarationUpdatedEventHandler>();
        //    For<IEventHandler<GenericEvent<AgreementSignedEvent>>>().Use<AgreementSignedEventHandler>();
        //    For<IEventHandler<PeriodEnd>>().Use<PeriodEndEventHandler>();
        //    For<IEventHandler<GenericEvent<EmploymentCheckCompleteEvent>>>().Use<EmploymentCheckCompleteEventHandler>();
        //    For<IEventHandler<AgreementEventView>>().Use<AgreementEventHandler>();

        //    //Legacy support
        //    For<IEventHandler<AccountEventView>>().Use<AccountEventHandler>();
        //}

        

       

        //private void RegisterRepositories(string connectionString)
        //{
        //    For<IEventRepository>().Use<EventRepository>().Ctor<string>().Is(connectionString);
        //    For<IAccountRepository>().Use<AccountRepository>().Ctor<string>().Is(connectionString);
        //    For<ILegalEntityRepository>().Use<LegalEntityRepository>().Ctor<string>().Is(connectionString);
        //    For<IPayeSchemeRepository>().Use<PayeSchemeRepository>().Ctor<string>().Is(connectionString);
        //    For<IApprenticeshipRepository>().Use<ApprenticeshipRepository>().Ctor<string>().Is(connectionString);
        //    For<IPaymentRepository>().Use<PaymentRepository>().Ctor<string>().Is(connectionString);
        //    For<ILevyDeclarationRepository>().Use<LevyDeclarationRepository>().Ctor<string>().Is(connectionString);
        //    For<IEmployerAgreementRepository>().Use<EmployerAgreementRepository>().Ctor<string>().Is(connectionString);
        //    For<IEmploymentCheckRepository>().Use<EmploymentCheckRepository>().Ctor<string>().Is(connectionString);
        //    For<IProviderRepository>().Use<ProviderRepository>().Ctor<string>().Is(connectionString);
        //}

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

        private static IConfigurationRepository GetConfigurationRepository()
        {
            return new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
        }

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null)).AlwaysUnique();
        }
    }
}
