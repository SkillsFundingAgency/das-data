using System.Linq;
using System.Net.Http;
using MediatR;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Http;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Client;
using StructureMap;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.Commitments.Api.Client;
using SFA.DAS.Commitments.Api.Client.Configuration;
using SFA.DAS.Commitments.Api.Client.Interfaces;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.NLog.Logger.Web.MessageHandlers;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;

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
                var assemblyNames = (typeof(DefaultRegistry).Assembly.GetReferencedAssemblies()).ToList().Where(w => w.FullName.StartsWith("SFA.DAS.")).Select( a => a.FullName);

                foreach (var assemblyName in assemblyNames)
                {
                    scan.Assembly(assemblyName);
                }
                
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            var config = GetConfiguration();

            For<IDataConfiguration>().Use(config);
            RegisterApis(config);
    
            RegisterRepositories(config.DatabaseConnectionString, config.PsrsDatabaseConnectionString);
            AddMediatrRegistrations();

            ConfigureLogging();
        }


        private void RegisterRepositories(string connectionString, string psrsConnectionString)
        {
            // Add registrations here
            For<IPsrsExternalRepository>().Use<PsrsExternalRepository>().Ctor<string>().Is(psrsConnectionString);
            For<IPsrsRepository>().Use<PsrsRepository>().Ctor<string>().Is(connectionString);
            For<ITransferRelationshipRepository>().Use<TransferRelationshipRepository>().Ctor<string>().Is(connectionString);
            For<IStatisticsRepository>().Use<StatisticsRepository>().Ctor<string>().Is(connectionString);

            HttpMessageHandler handler = new HttpClientHandler();
            For<IHttpClientWrapper>().Use<HttpClientWrapper>().Ctor<HttpMessageHandler>().Is(handler);

            For<IStatisticsService>().Use<StatisticsService>();
            For<IPsrsReportsService>().Use<PsrsReportsService>();
            For<ITransferRelationshipService>().Use<TransferRelationshipMessageService>();
        }

        private DataConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

            var configurationRepository = GetConfigurationRepository();
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, Version));

            return configurationService.Get<DataConfiguration>();
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

        private static IConfigurationRepository GetConfigurationRepository()
        {
            return new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
        }

        private void ConfigureLogging()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null,null)).AlwaysUnique();
        }

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }
    }

}
