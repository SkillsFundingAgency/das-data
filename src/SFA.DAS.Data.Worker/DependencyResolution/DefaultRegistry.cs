using MediatR;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.Data.Worker.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.Data";
        private const string Version = "1.0";

        public DefaultRegistry()
        {

            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            var config = GetConfiguration();

            RegisterRepositories(config.DatabaseConnectionString);
            RegisterApis(config);

            AddMediatrRegistrations();
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IEventsApi>().Use<EventsApi>().Ctor<IEventsApiClientConfiguration>().Is(config.EventsApi);
        }

        private void RegisterRepositories(string connectionString)
        {
            For<IEventRepository>().Use<EventRepository>().Ctor<string>().Is(connectionString);
            For<IRegistrationRepository>().Use<RegistrationRepository>().Ctor<string>().Is(connectionString);
        }

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }

        private DataConfiguration GetConfiguration()
        {
            return new DataConfiguration();
        }
    }
}