using System.Configuration;
using Microsoft.Azure;
using NUnit.Framework;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.Data.AcceptanceTests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        private const string ServiceName = "SFA.DAS.Data.AcceptanceTests";

        [OneTimeSetUp]
        public void SetUp()
        {
            var applicationConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var azureConfig = GetAzureStorageConfig();
            applicationConfig.AppSettings.Settings["DataConnectionString"].Value = azureConfig.DataConnectionString;
            applicationConfig.AppSettings.Settings["AccountsApiClientId"].Value = azureConfig.AccountApiConfiguration.ClientId;
            applicationConfig.AppSettings.Settings["AccountsApiClientSecret"].Value = azureConfig.AccountApiConfiguration.ClientSecret;
            applicationConfig.AppSettings.Settings["AccountsApiIdentifierUri"].Value = azureConfig.AccountApiConfiguration.IdentifierUri;
            applicationConfig.AppSettings.Settings["AccountsApiTenant"].Value = azureConfig.AccountApiConfiguration.Tenant;
            applicationConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private AcceptanceTestConfiguration GetAzureStorageConfig()
        {
            var configurationRepository = new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, "1.0"));
            return configurationService.Get<AcceptanceTestConfiguration>();
        }
    }
}
