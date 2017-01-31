using System.Configuration;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    public abstract class AccountEventTestsBase
    {
        protected WorkerRole WorkerRole;
        protected WebApiSubstitute EventsApi;
        protected WebApiSubstitute AccountsApi;
        protected EventTestsRepository EventTestsRepository;

        [SetUp]
        public void Arrange()
        {
            StartSubstituteApis();
            StartWorkerRole();
            SetupDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            EventsApi.Dispose();
            AccountsApi.Dispose();
        }

        private void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(ConfigurationManager.AppSettings["DataConnectionString"]);
            EventTestsRepository.DeleteAccounts().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId("AccountEvents", 2).Wait();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole();
            WorkerRole.OnStart();
        }

        private void StartSubstituteApis()
        {
            EventsApi = new WebApiSubstitute(ConfigurationManager.AppSettings["EventsApiBaseUrl"]);
            AccountsApi = new WebApiSubstitute(ConfigurationManager.AppSettings["AccountsApiBaseUrl"]);

            EventsApi.Start();
            AccountsApi.Start();
        }
    }
}
