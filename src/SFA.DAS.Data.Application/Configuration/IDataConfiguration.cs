using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.Application.Configuration
{
    public interface IDataConfiguration
    {
        AccountApiConfiguration AccountsApi { get; }
        string DatabaseConnectionString { get; }
        EventsApiClientConfiguration EventsApi { get; }
        int FailureTolerance { get; }
    }
}