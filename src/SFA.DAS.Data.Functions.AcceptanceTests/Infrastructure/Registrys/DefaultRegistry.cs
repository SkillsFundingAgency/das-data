using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using MediatR;
using Microsoft.Azure;
using SFA.DAS.Commitments.Api.Client;
using SFA.DAS.Commitments.Api.Client.Configuration;
using SFA.DAS.Commitments.Api.Client.Interfaces;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.AcceptanceTests.Stubs;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Http;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NLog.Logger.Web.MessageHandlers;
using SFA.DAS.Provider.Events.Api.Client;
using StructureMap;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure.Registrys
{
    public class DefaultRegistry : Registry
    {
        private const string ServiceNamespace = "SFA.DAS";
        private string ServiceName = CloudConfigurationManager.GetSetting("ServiceName");
        private const string Version = "1.0";

        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceNamespace));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            ForSingletonOf<Config>().Use(new Config());
           
            For<IDbConnection>()
                .Use<SqlConnection>()
                .SelectConstructor(() =>
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"]
                        .ConnectionString))
                .Ctor<string>("connectionString")
                .Is(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString);

            AddMediatrRegistrations();
            
            ConfigureLogging();
            SetupStubs();
            For<IHttpClientWrapper>().Use<HttpClientWrapper>().SelectConstructor(() => new HttpClientWrapper());
            For<IStatisticsService>().Use<StatisticsService>();

            var config = GetConfiguration();
            For<IStatisticsRepository>().Use<StatisticsRepository>().Ctor<string>().Is(config.DatabaseConnectionString);
            For<IEventsApi>().Use(new EventsApi(config.EventsApi));
            For<IDataConfiguration>().Use(config);
            RegisterApis(config);
        }

        private void SetupStubs()
        {
            For<IEasStatisticsHandler>().Use<StubEasStatisticsHandler>();
            For<ICommitmentsStatisticsHandler>().Use<StubCommitmentsStatisticsHandler>();
            For<IPaymentStatisticsHandler>().Use<StubPaymentsStatisticsHandler>();
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

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null,null)).AlwaysUnique();
        }

        private void RegisterApis(DataConfiguration config)
        {
            For<IPaymentsEventsApiClient>().Use(new PaymentsEventsApiClient(config.PaymentsEvents));
            For<IAccountApiClient>().Use<AccountApiClient>().Ctor<IAccountApiConfiguration>().Is(config.AccountsApi);

            IJwtClientConfiguration clientConfig = config.CommitmentsApi;

            var bearerToken = (IGenerateBearerToken)new JwtBearerTokenGenerator(clientConfig);

            var httpClient = new HttpClientBuilder()
                .WithBearerAuthorisationHeader(bearerToken)
                .WithHandler(new RequestIdMessageRequestHandler())
                .WithHandler(new SessionIdMessageRequestHandler())
                .WithDefaultHeaders()
                .Build();

            For<IEmployerCommitmentApi>().Use<EmployerCommitmentApi>()
                .Ctor<HttpClient>().Is(httpClient)
                .Ctor<ICommitmentsApiClientConfiguration>().Is(config.CommitmentsApi);
            For<IStatisticsApi>().Use<StatisticsApi>().Ctor<HttpClient>().Is(httpClient).Ctor<ICommitmentsApiClientConfiguration>().Is(config.CommitmentsApi);
        }

    }
}
