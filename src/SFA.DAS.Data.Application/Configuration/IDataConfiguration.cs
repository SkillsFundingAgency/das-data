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
        bool TransfersEnabled { get; set; }
        PerformancePlatformConfiguration PerformancePlatform { get; set; }
        string AgreementsApiUrl { get; set; }
        string EasStatisticsEndPoint { get; set; }
        string CommitmentsStatisticsEndPoint { get; set; }
        string PaymentsStatisticsEndPoint { get; set; }
    }
}