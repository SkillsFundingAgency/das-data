using Microsoft.Azure;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Provider.Events.Api.Client;

namespace SFA.DAS.Data.Application.Configuration
{
    public class DataConfiguration : IDataConfiguration
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

        public PaymentsEventsApiConfiguration PaymentsEvents => new PaymentsEventsApiConfiguration
        {
            ApiBaseUrl = CloudConfigurationManager.GetSetting("PaymentsEventsApiBaseUrl", false),
            ClientToken = CloudConfigurationManager.GetSetting("PaymentsEventsApiClientToken", false)
        };
    }
}
