using Microsoft.Azure;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.Application.Configuration
{
    public class DataConfiguration
    {
        public string DatabaseConnectionString => CloudConfigurationManager.GetSetting("DataConnectionString", false);

        public int FailureTolerance => int.Parse(CloudConfigurationManager.GetSetting("EventFailureTolerance", false));

        public EventsApiClientConfiguration EventsApi => new EventsApiClientConfiguration();

        public AccountApiConfiguration AccountsApi => new AccountApiConfiguration
        {
            ApiBaseUrl = CloudConfigurationManager.GetSetting("AccountsApiBaseUrl", false),
            ClientId = CloudConfigurationManager.GetSetting("AccountsApiClientId", false),
            ClientSecret = CloudConfigurationManager.GetSetting("AccountsApiClientSecret", false),
            IdentifierUri = CloudConfigurationManager.GetSetting("AccountsApiIdentifierUri", false),
            Tenant = CloudConfigurationManager.GetSetting("AccountsApiTenant", false),
        };
    }
}
