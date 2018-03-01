using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests
{
    public abstract class EventTestBase
    {
        protected WorkerRole WorkerRole;
        protected EventTestsRepository EventTestsRepository;

        protected abstract string EventName { get; }

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

        protected abstract void SetupDatabase();

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole
            {
                UseEventsApi = true,
                UseMessageProcessors = false
            };
            WorkerRole.OnStart();
        }

        private void ClearSubstituteApis()
        {
            DataAcceptanceTests.ClearApiSetup();
        }
    }
}
