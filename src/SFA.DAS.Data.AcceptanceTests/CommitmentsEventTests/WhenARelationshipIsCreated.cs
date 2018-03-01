using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.AcceptanceTests.CommitmentsEventTests
{
    [TestFixture]
    public class WhenARelationshipIsCreated : CommitmentsEventTestBase
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
    }
}
