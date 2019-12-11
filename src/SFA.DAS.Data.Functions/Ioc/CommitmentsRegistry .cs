using System.Net.Http;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.Application.Configuration;
using StructureMap;
using SFA.DAS.Commitments.Api.Client;
using SFA.DAS.Commitments.Api.Client.Configuration;
using SFA.DAS.Commitments.Api.Client.Interfaces;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;

namespace SFA.DAS.Data.Functions.Ioc
{
    public class CommitmentsRegistry : Registry
    {
        private string ServiceName = "SFA.DAS.CommitmentsAPI";
        private const string Version = "1.0";

        public CommitmentsRegistry()
        {
            For<IEmployerCommitmentApi>().Use<EmployerCommitmentApi>()
                .Ctor<HttpClient>().Is(ctx => GetHttpClient(ctx))
                .Ctor<ICommitmentsApiClientConfiguration>().Is(GetCommitmentsConfiguration());
            For<IStatisticsApi>().Use<StatisticsApi>().Ctor<HttpClient>().Is(ctx => GetHttpClient(ctx)).Ctor<ICommitmentsApiClientConfiguration>().Is(GetCommitmentsConfiguration());
        }

        private CommitmentsApiClientConfiguration GetCommitmentsConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

            var configurationRepository = GetConfigurationRepository();
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, Version));

            return configurationService.Get<CommitmentsApiClientConfiguration>();
        }

        private HttpClient GetHttpClient(IContext context)
        {         
            var config = GetCommitmentsConfiguration();
            var bearerToken = (IGenerateBearerToken)new JwtBearerTokenGenerator(config);

            var httpClientBuilder = string.IsNullOrWhiteSpace(config.ClientId)
               ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(config))
               : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureActiveDirectoryBearerTokenGenerator(config));

            return httpClientBuilder
                .WithDefaultHeaders()
                .Build();         
        }

        private static IConfigurationRepository GetConfigurationRepository()
        {
            return new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
        }
    }
}
