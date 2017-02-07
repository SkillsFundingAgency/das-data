using System.Configuration;
using Microsoft.Azure;
using NUnit.Framework;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EAS.Account.Api.Client;

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
            applicationConfig.AppSettings.Settings["AccountsApiClientId"].Value = azureConfig.ClientId;
            applicationConfig.AppSettings.Settings["AccountsApiClientSecret"].Value = azureConfig.ClientSecret;
            applicationConfig.AppSettings.Settings["AccountsApiIdentifierUri"].Value = azureConfig.IdentifierUri;
            applicationConfig.AppSettings.Settings["AccountsApiTenant"].Value = azureConfig.Tenant;
            applicationConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private AccountApiConfiguration GetAzureStorageConfig()
        {
            var configurationRepository = new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(ServiceName, environment, "1.0"));
            return configurationService.Get<AccountApiConfiguration>();
        }
    }
}
