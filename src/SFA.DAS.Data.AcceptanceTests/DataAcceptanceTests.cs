using Microsoft.Azure;
using NUnit.Framework;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.Application.Configuration;

namespace SFA.DAS.Data.AcceptanceTests
{
    [SetUpFixture]
    public class DataAcceptanceTests
    {
        internal static WebApiSubstitute EventsApi;
        internal static WebApiSubstitute AccountsApi;
        internal static WebApiSubstitute ProviderEventsApi;
        internal static DataConfiguration Config;

        [OneTimeSetUp]
        public void SetUp()
        {
            StartSubstituteApis();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            EventsApi.Dispose();
            AccountsApi.Dispose();
            ProviderEventsApi.Dispose();
        }

        public static void ClearApiSetup()
        {
            EventsApi.ClearSetup();
            AccountsApi.ClearSetup();
            ProviderEventsApi.ClearSetup();
        }

        private static void StartSubstituteApis()
        {
            Config = GetAzureStorageConfig();

            EventsApi = new WebApiSubstitute(Config.EventsApi.BaseUrl);
            AccountsApi = new WebApiSubstitute(Config.AccountsApi.ApiBaseUrl);
            ProviderEventsApi = new WebApiSubstitute(Config.PaymentsEvents.ApiBaseUrl);

            EventsApi.Start();
            AccountsApi.Start();
            ProviderEventsApi.Start();
        }

        private static DataConfiguration GetAzureStorageConfig()
        {
            var configurationRepository = new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString", false));
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName", false);
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(CloudConfigurationManager.GetSetting("ServiceName", false), environment, "1.0"));
            return configurationService.Get<DataConfiguration>();
        }
    }
}
