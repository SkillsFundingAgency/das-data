using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using MediatR;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types.Events;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;
using StructureMap;
using StructureMap.TypeRules;

namespace SFA.DAS.Data.Worker.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            var config = GetConfiguration();

            For<IEventsWatcher>().Use<EventsWatcher>().Ctor<int>().Is(config.FailureTolerance);
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
            For<IEventHandler<AccountCreatedEvent>>().Use<AccountCreatedEventHandler>();
            For<IEventHandler<AccountRenamedEvent>>().Use<AccountRenamedEventHandler>();
            For<IEventHandler<ApprenticeshipEventView>>().Use<ApprenticeshipEventHandler>();
            For<IEventHandler<LegalEntityCreatedEvent>>().Use<LegalEntityCreatedEventHandler>();
            For<IEventHandler<PayeSchemeAddedEvent>>().Use<PayeSchemeAddedEventHandler>();
            For<IEventHandler<PayeSchemeRemovedEvent>>().Use<PayeSchemeRemovedEventHandler>();

            //Legacy support
            For<IEventHandler<AccountEventView>>().Use<AccountEventHandler>();
        }

        private void RegisterEventCollectors()
        {
            For<IEventsCollector<AccountCreatedEvent>>().Use<GenericEventCollector<AccountCreatedEvent>>();
            For<IEventsCollector<AccountRenamedEvent>>().Use<GenericEventCollector<AccountRenamedEvent>>();
            For<IEventsCollector<ApprenticeshipEventView>>().Use<ApprenticeshipEventsCollector>();
            For<IEventsCollector<LegalEntityCreatedEvent>>().Use<GenericEventCollector<LegalEntityCreatedEvent>>();
            For<IEventsCollector<PayeSchemeAddedEvent>>().Use<GenericEventCollector<PayeSchemeAddedEvent>>();
            For<IEventsCollector<PayeSchemeRemovedEvent>>().Use<GenericEventCollector<PayeSchemeRemovedEvent>>();

            //Legacy support
            For<IEventsCollector<AccountEventView>>().Use<AccountEventCollector>();
        }

        private void RegisterEventProcessors()
        {
            For<IEventsProcessor>().Use<EventsProcessor<AccountCreatedEvent>>();
            For<IEventsProcessor>().Use<EventsProcessor<AccountRenamedEvent>>();
            For<IEventsProcessor>().Use<EventsProcessor<ApprenticeshipEventView>>();
            For<IEventsProcessor>().Use<EventsProcessor<LegalEntityCreatedEvent>>();
            For<IEventsProcessor>().Use<EventsProcessor<PayeSchemeAddedEvent>>();
            For<IEventsProcessor>().Use<EventsProcessor<PayeSchemeRemovedEvent>>();

            //Legacy support
            For<IEventsProcessor>().Use<EventsProcessor<AccountEventView>>();
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IEventsApi>().Use(new EventsApi(config.EventsApi));
            
            For<IAccountApiClient>().Use<AccountApiClient>().Ctor<IAccountApiConfiguration>().Is(config.AccountsApi);
        }

        private void RegisterRepositories(string connectionString)
        {
            For<IEventRepository>().Use<EventRepository>().Ctor<string>().Is(connectionString);
            For<IAccountRepository>().Use<AccountRepository>().Ctor<string>().Is(connectionString);
            For<ILegalEntityRepository>().Use<LegalEntityRepository>().Ctor<string>().Is(connectionString);
            For<IPayeSchemeRepository>().Use<PayeSchemeRepository>().Ctor<string>().Is(connectionString);
            For<IApprenticeshipRepository>().Use<ApprenticeshipRepository>().Ctor<string>().Is(connectionString);
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
            return new DataConfiguration();
        }

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null)).AlwaysUnique();
        }
    }
}