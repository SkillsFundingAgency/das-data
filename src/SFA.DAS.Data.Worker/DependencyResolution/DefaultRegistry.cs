using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
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
using SFA.DAS.EAS.Account.Api.Types.Events.LegalEntity;
using SFA.DAS.EAS.Account.Api.Types.Events.Levy;
using SFA.DAS.EAS.Account.Api.Types.Events.PayeScheme;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Client;
using SFA.DAS.Provider.Events.Api.Types;
using StructureMap;
using StructureMap.TypeRules;

namespace SFA.DAS.Data.Worker.DependencyResolution
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
            RegisterApis(config);

            RegisterEventCollectors();
            RegisterEventHandlers();
            RegisterEventProcessors();

            RegisterMapper();

            AddMediatrRegistrations();

            ConfigureLogging();
        }
        
        private void RegisterEventHandlers()
        {
            For<IEventHandler<GenericEvent<AccountCreatedEvent>>>().Use<AccountCreatedEventHandler>();
            For<IEventHandler<GenericEvent<AccountRenamedEvent>>>().Use<AccountRenamedEventHandler>();
            For<IEventHandler<ApprenticeshipEventView>>().Use<ApprenticeshipEventHandler>();
            For<IEventHandler<GenericEvent<LegalEntityCreatedEvent>>>().Use<LegalEntityCreatedEventHandler>();
            For<IEventHandler<GenericEvent<PayeSchemeAddedEvent>>>().Use<PayeSchemeAddedEventHandler>();
            For<IEventHandler<GenericEvent<PayeSchemeRemovedEvent>>>().Use<PayeSchemeRemovedEventHandler>();
            For<IEventHandler<GenericEvent<LevyDeclarationUpdatedEvent>>>().Use<LevyDeclarationUpdatedEventHandler>();
            For<IEventHandler<PeriodEnd>>().Use<PeriodEndEventHandler>();

            //Legacy support
            For<IEventHandler<AccountEventView>>().Use<AccountEventHandler>();
        }

        private void RegisterEventCollectors()
        {
            For<IEventsCollector<GenericEvent<AccountCreatedEvent>>>().Use<GenericEventCollector<AccountCreatedEvent>>();
            For<IEventsCollector<GenericEvent<AccountRenamedEvent>>>().Use<GenericEventCollector<AccountRenamedEvent>>();
            For<IEventsCollector<ApprenticeshipEventView>>().Use<ApprenticeshipEventsCollector>();
            For<IEventsCollector<GenericEvent<LegalEntityCreatedEvent>>>().Use<GenericEventCollector<LegalEntityCreatedEvent>>();
            For<IEventsCollector<GenericEvent<PayeSchemeAddedEvent>>>().Use<GenericEventCollector<PayeSchemeAddedEvent>>();
            For<IEventsCollector<GenericEvent<PayeSchemeRemovedEvent>>>().Use<GenericEventCollector<PayeSchemeRemovedEvent>>();
            For<IEventsCollector<GenericEvent<LevyDeclarationUpdatedEvent>>>().Use<GenericEventCollector<LevyDeclarationUpdatedEvent>>();
            For<IEventsCollector<PeriodEnd>>().Use<PaymentEventsCollector>();

            //Legacy support
            For<IEventsCollector<AccountEventView>>().Use<AccountEventCollector>();
        }

        private void RegisterEventProcessors()
        {
            For<IEventsProcessor>().Use<EventsProcessor<GenericEvent<AccountCreatedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<GenericEvent<AccountRenamedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<ApprenticeshipEventView>>();
            For<IEventsProcessor>().Use<EventsProcessor<GenericEvent<LegalEntityCreatedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<GenericEvent<PayeSchemeAddedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<GenericEvent<PayeSchemeRemovedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<GenericEvent<LevyDeclarationUpdatedEvent>>>();
            For<IEventsProcessor>().Use<EventsProcessor<PeriodEnd>>();

            //Legacy support
            For<IEventsProcessor>().Use<EventsProcessor<AccountEventView>>();
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IEventsApi>().Use(new EventsApi(config.EventsApi));
            For<IPaymentsEventsApiClient>().Use(new PaymentsEventsApiClient(config.PaymentsEvents));
            For<IAccountApiClient>().Use<AccountApiClient>().Ctor<IAccountApiConfiguration>().Is(config.AccountsApi);
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
        }

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }

        private void RegisterMapper()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("SFA.DAS.Data"));

            var mappingProfiles = new List<Profile>();

            foreach (var assembly in assemblies)
            {
                var profiles = Assembly.Load(assembly.FullName).GetTypes()
                                       .Where(t => typeof(Profile).IsAssignableFrom(t))
                                       .Where(t => t.IsConcrete() && t.HasConstructors())
                                       .Select(t => (Profile)Activator.CreateInstance(t));

                mappingProfiles.AddRange(profiles);
            }

            var config = new MapperConfiguration(cfg =>
            {
                mappingProfiles.ForEach(cfg.AddProfile);
            });

            var mapper = config.CreateMapper();

            For<IConfigurationProvider>().Use(config).Singleton();
            For<IMapper>().Use(mapper).Singleton();
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