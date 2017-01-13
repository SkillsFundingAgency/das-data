using Microsoft.Azure;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.Application.Configuration
{
    public class DataConfiguration
    {
        public string DatabaseConnectionString => CloudConfigurationManager.GetSetting("DataConnectionString");

        public EventsApiClientConfiguration EventsApi => new EventsApiClientConfiguration();

        public AccountApiConfiguration AccountsApi => new AccountApiConfiguration
        {
            ApiBaseUrl = CloudConfigurationManager.GetSetting("AccountsApiBaseUrl"),
            ClientId = CloudConfigurationManager.GetSetting("AccountsApiClientId"),
            ClientSecret = CloudConfigurationManager.GetSetting("AccountsApiClientSecret"),
            IdentifierUri = CloudConfigurationManager.GetSetting("AccountsApiIdentifierUri"),
            Tenant = CloudConfigurationManager.GetSetting("AccountsApiTenant"),
        };
    }
}
