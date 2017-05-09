using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.Application.Configuration
{
    public interface IDataConfiguration
    {
        AccountApiConfiguration AccountsApi { get; set; }
        string DatabaseConnectionString { get; set; }
        EventsApiClientConfiguration EventsApi { get; set; }
        int FailureTolerance { get; set; }
        bool PaymentsEnabled { get; set; }
    }
}