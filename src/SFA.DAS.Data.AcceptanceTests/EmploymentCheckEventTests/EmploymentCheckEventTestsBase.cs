using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.EmploymentCheckEventTests
{
    public abstract class EmploymentCheckEventTestsBase
    {
        protected WorkerRole WorkerRole;
        protected EventTestsRepository EventTestsRepository;

        protected WebApiSubstitute EventsApi => DataAcceptanceTests.EventsApi;

        [SetUp]
        public void Arrange()
        {
            ClearSubstituteApis();
            StartWorkerRole();
            SetupDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            Task.Run(() => WorkerRole?.OnStop());
            WorkerRole = null;
        }

        private void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteEmploymentChecks().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId("EmploymentCheckCompleteEvent", 2).Wait();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole();
            WorkerRole.OnStart();
        }

        private void ClearSubstituteApis()
        {
            DataAcceptanceTests.ClearApiSetup();
        }
    }
}
