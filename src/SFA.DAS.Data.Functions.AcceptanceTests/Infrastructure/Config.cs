using System;
using System.Configuration;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure
{
    public class Config
    {
        public TimeSpan TimeToWait => TimeSpan.Parse(ConfigurationManager.AppSettings["TimeToWait"] ?? "00:00:30");
        public TimeSpan TimeToPause => TimeSpan.Parse(ConfigurationManager.AppSettings["TimeToPause"] ?? "00:00:05");
        public string Environment => GetAppSetting("Environment");

        public bool IsDevEnvironment => (Environment?.Equals("DEVELOPMENT", StringComparison.OrdinalIgnoreCase) ?? false) ||
                                        (Environment?.Equals("LOCAL", StringComparison.OrdinalIgnoreCase) ?? false);
        public string LevyFunctionUrl => GetAppSetting("LevyFunctionUrl");
        public string LevyPreLoadFunctionUrl => GetAppSetting("LevyPreLoadFunctionUrl");
        public string PaymentFunctionUrl => GetAppSetting("PaymentFunctionUrl");
        public string PaymentPreLoadHttpFunction => GetAppSetting("PaymentPreLoadHttpFunction");
        public string ProjectionPaymentFunctionUrl => GetAppSetting("ProjectionPaymentFunctionUrl");
        public string ProjectionLevyFunctionUrl => GetAppSetting("ProjectionLevyFunctionUrl");
        public int EmployerAccountId => int.Parse(GetAppSetting("EmployerAccountId"));
        public string AzureStorageConnectionString => GetConnectionString("StorageConnectionString");
        public string ApiInsertBalanceUrl => GetAppSetting("ApiInsertBalanceUrl");
        public string ApiInsertPaymentUrl => GetAppSetting("ApiInsertPaymentUrl");
        public string ApiInsertLevyUrl => GetAppSetting("ApiInsertLevyUrl");
        public string StubEmployerPaymentTable => GetAppSetting("Stub-EmployerPaymentTable");

        public string EventsApi => GetAppSetting("EventsApi");

        protected string GetAppSetting(string keyName) => ConfigurationManager.AppSettings[keyName] ?? throw new InvalidOperationException($"{keyName} not found in app settings.");
        protected string GetConnectionString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString ?? throw new InvalidOperationException($"{name} not found in connection strings.");

    }
}
