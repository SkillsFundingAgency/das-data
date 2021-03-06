﻿using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Provider.Events.Api.Client;

namespace SFA.DAS.Data.Application.Configuration
{
    public class DataConfiguration : IDataConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string PsrsDatabaseConnectionString { get; set; }
        public int FailureTolerance { get; set; }
        public EventsApiClientConfiguration EventsApi { get; set; }
        public AccountApiConfiguration AccountsApi { get; set; }
        public PaymentsEventsApiConfiguration PaymentsEvents { get; set; }
        public bool PaymentsEnabled { get; set; }
        public bool TransfersEnabled { get; set; }
        public bool DataLocksEnabled { get; set; }
        public PerformancePlatformConfiguration PerformancePlatform { get; set; }
        public string AgreementsApiUrl { get; set; }
        public string EasStatisticsEndPoint { get; set; }
        public string CommitmentsStatisticsEndPoint { get; set; }
        public string PaymentsStatisticsEndPoint { get; set; }
        public CommitmentsApiClientConfiguration CommitmentsApi { get; set; }
       
    }
}
