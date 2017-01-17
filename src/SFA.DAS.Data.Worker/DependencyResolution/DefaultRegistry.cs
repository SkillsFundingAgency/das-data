using MediatR;
using Microsoft.WindowsAzure.Storage.File;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Client.Configuration;
using SFA.DAS.NLog.Logger;
using StructureMap;

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

            For<IEventProcessor>().Use<EventProcessor>().Ctor<int>().Is(config.FailureTolerance);
            RegisterRepositories(config.DatabaseConnectionString);
            RegisterApis(config);

            AddMediatrRegistrations();

            ConfigureLogging();
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IEventsApi>().Use(new EventsApi(config.EventsApi));
            
            For<IAccountApiClient>().Use<AccountApiClient>().Ctor<AccountApiConfiguration>().Is(config.AccountsApi);
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

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null)).AlwaysUnique();
        }
    }
}