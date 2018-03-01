using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.DependencyResolution;
using SFA.DAS.Data.Worker;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests
{
    public abstract class MessageTestBase
    {
        protected WorkerRole WorkerRole;
        protected EventTestsRepository EventTestsRepository;
        protected IAzureTopicMessageBus AzureTopicMessageBus;

        protected IContainer Container;

        protected abstract void SetupDatabase();
        protected abstract void SetupContainer();

        [SetUp]
        public void Arrange()
        {
            SetupContainer();

            SetupDatabase();
            StartWorkerRole();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole
            {
                UseEventsApi = false,
                UseMessageProcessors = true
            };
            WorkerRole.OnStart();
        }
    }
}
