using SFA.DAS.Data.Worker;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.CommitmentsEventTests
{
    public abstract class CommitmentsEventTestBase
    {
        protected WorkerRole WorkerRole;
        protected EventTestsRepository EventTestsRepository;
        protected IAzureTopicMessageBus AzureTopicMessageBus;
        private IContainer _container;

        [SetUp]
        public void Arrange()
        {
            SetupContainer();

            SetupDatabase();
            StartWorkerRole();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole();
            WorkerRole.OnStart();
        }

        private void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteRelationships().Wait();
        }

        private void SetupContainer()
        {
            _container = new Container(c=> c.AddRegistry<TestRegistry>());

            AzureTopicMessageBus = _container.GetInstance<IAzureTopicMessageBus>();
        }
    }
}
