using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.DependencyResolution;
using SFA.DAS.Data.Tests.Builders;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.CommitmentsEventTests
{
    [TestFixture]
    public class WhenARelationshipIsCreated : MessageTestBase
    {
        private readonly RelationshipCreated _relationshipCreated = new RelationshipCreatedBuilder().Build();

        [Test]
        public void ThenTheRelationshipDetailsAreStored()
        {
            AzureTopicMessageBus.PublishAsync(_relationshipCreated);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var numberOfRelationships = await EventTestsRepository.CheckRelationshipCreated(_relationshipCreated);
            return numberOfRelationships == 1;
        }

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteRelationships().Wait();
        }

        protected override void SetupContainer()
        {
            Container = new Container(c => c.AddRegistry<TestRegistry>());

            AzureTopicMessageBus = Container.GetInstance<IAzureTopicMessageBus>();
        }
    }
}
