using MediatR;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types.Events.Account;
using SFA.DAS.EAS.Account.Api.Types.Events.Agreement;
using SFA.DAS.EAS.Account.Api.Types.Events.LegalEntity;
using SFA.DAS.EAS.Account.Api.Types.Events.Levy;
using SFA.DAS.EAS.Account.Api.Types.Events.PayeScheme;
using SFA.DAS.EmploymentCheck.Events;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Client;
using SFA.DAS.Provider.Events.Api.Types;
using SFA.Roatp.Api.Client;
using StructureMap;

namespace SFA.DAS.Data.Worker.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        private readonly string _serviceName = CloudConfigurationManager.GetSetting("ServiceName");
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
            RegisterApis(config);

            RegisterEventCollectors();
            RegisterEventHandlers();
            RegisterEventProcessors();

            AddMediatrRegistrations();

            ConfigureLogging();
        }
        
        private void RegisterEventHandlers()
        {
            For<IEventHandler<Events.GenericEvent<AccountCreatedEvent>>>().Use<AccountCreatedEventHandler>();
            For<IEventHandler<Events.GenericEvent<AccountRenamedEvent>>>().Use<AccountRenamedEventHandler>();
            For<IEventHandler<ApprenticeshipEventView>>().Use<ApprenticeshipEventHandler>();
            For<IEventHandler<Events.GenericEvent<LegalEntityCreatedEvent>>>().Use<LegalEntityCreatedEventHandler>();
            For<IEventHandler<Events.GenericEvent<PayeSchemeAddedEvent>>>().Use<PayeSchemeAddedEventHandler>();
            For<IEventHandler<Events.GenericEvent<PayeSchemeRemovedEvent>>>().Use<PayeSchemeRemovedEventHandler>();
            For<IEventHandler<Events.GenericEvent<LevyDeclarationUpdatedEvent>>>().Use<LevyDeclarationUpdatedEventHandler>();
            For<IEventHandler<Events.GenericEvent<AgreementSignedEvent>>>().Use<AgreementSignedEventHandler>();
            For<IEventHandler<PeriodEnd>>().Use<PeriodEndEventHandler>();
            For<IEventHandler<Events.GenericEvent<EmploymentCheckCompleteEvent>>>().Use<EmploymentCheckCompleteEventHandler>();
            For<IEventHandler<AgreementEventView>>().Use<AgreementEventHandler>();

            //Legacy support
            For<IEventHandler<AccountEventView>>().Use<AccountEventHandler>();
        }

        private void RegisterEventCollectors()
        {
            For<IEventsCollector<Events.GenericEvent<AccountCreatedEvent>>>().Use<GenericEventCollector<AccountCreatedEvent>>();
            For<IEventsCollector<Events.GenericEvent<AccountRenamedEvent>>>().Use<GenericEventCollector<AccountRenamedEvent>>();
            For<IEventsCollector<ApprenticeshipEventView>>().Use<ApprenticeshipEventsCollector>();
            For<IEventsCollector<Events.GenericEvent<LegalEntityCreatedEvent>>>().Use<GenericEventCollector<LegalEntityCreatedEvent>>();
            For<IEventsCollector<Events.GenericEvent<PayeSchemeAddedEvent>>>().Use<GenericEventCollector<PayeSchemeAddedEvent>>();
            For<IEventsCollector<Events.GenericEvent<PayeSchemeRemovedEvent>>>().Use<GenericEventCollector<PayeSchemeRemovedEvent>>();
            For<IEventsCollector<Events.GenericEvent<LevyDeclarationUpdatedEvent>>>().Use<GenericEventCollector<LevyDeclarationUpdatedEvent>>();
            For<IEventsCollector<Events.GenericEvent<AgreementSignedEvent>>>().Use<GenericEventCollector<AgreementSignedEvent>>();
            For<IEventsCollector<PeriodEnd>>().Use<PaymentEventsCollector>();
            For<IEventsCollector<Events.GenericEvent<EmploymentCheckCompleteEvent>>>().Use<GenericEventCollector<EmploymentCheckCompleteEvent>>();
            // Following code commented out so that Roatp changes do not go through to live as there are some questions around the tesing - Mahinder Suniara
            // For<IEventsCollector<AgreementEventView>>().Use<AgreementEventCollector>();

            //Legacy support
            For<IEventsCollector<AccountEventView>>().Use<AccountEventCollector>();
        }

        private void RegisterEventProcessors()
        {
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<AccountCreatedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<AccountRenamedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<ApprenticeshipEventView>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<LegalEntityCreatedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<PayeSchemeAddedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<PayeSchemeRemovedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<LevyDeclarationUpdatedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<AgreementSignedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<PeriodEnd>>();
            For<IEventsProcessor>().Use<EventsProcessor<Events.GenericEvent<EmploymentCheckCompleteEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<AgreementEventView>>();

            //Legacy support
            For<IEventsProcessor>().Use<EventsProcessor<AccountEventView>>();
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IEventsApi>().Use(new EventsApi(config.EventsApi));
            For<IPaymentsEventsApiClient>().Use(new PaymentsEventsApiClient(config.PaymentsEvents));
            For<IAccountApiClient>().Use<AccountApiClient>().Ctor<IAccountApiConfiguration>().Is(config.AccountsApi);
            For<IRoatpClient>().Use(new RoatpApiClient(config.AgreementsApiUrl));
        }

        private void RegisterRepositories(string connectionString)
        {
            For<IEventRepository>().Use<EventRepository>().Ctor<string>().Is(connectionString);
            For<IAccountRepository>().Use<AccountRepository>().Ctor<string>().Is(connectionString);
            For<ILegalEntityRepository>().Use<LegalEntityRepository>().Ctor<string>().Is(connectionString);
            For<IPayeSchemeRepository>().Use<PayeSchemeRepository>().Ctor<string>().Is(connectionString);
            For<IApprenticeshipRepository>().Use<ApprenticeshipRepository>().Ctor<string>().Is(connectionString);
            For<IPaymentRepository>().Use<PaymentRepository>().Ctor<string>().Is(connectionString);
            For<ILevyDeclarationRepository>().Use<LevyDeclarationRepository>().Ctor<string>().Is(connectionString);
            For<IEmployerAgreementRepository>().Use<EmployerAgreementRepository>().Ctor<string>().Is(connectionString);
            For<IEmploymentCheckRepository>().Use<EmploymentCheckRepository>().Ctor<string>().Is(connectionString);
            For<IProviderRepository>().Use<ProviderRepository>().Ctor<string>().Is(connectionString);
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
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(_serviceName, environment, Version));

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