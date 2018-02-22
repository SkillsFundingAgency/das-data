using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.AcceptanceTests.CommitmentsEventTests
{
    [TestFixture]
    public class WhenARelationshipIsVerified : CommitmentsEventTestBase
    {
        [Test]
        public void ThenTheRelationshipIsVerified()
        {
            InsertRowToUpdate();
            AzureTopicMessageBus.PublishAsync(new RelationshipVerifiedBuilder().Build());

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private void InsertRowToUpdate()
        {
            EventTestsRepository.InsertRelationship(new RelationshipBuilder().Build()).Wait();
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var numberOfVerifiedRelationships =
                await EventTestsRepository.GetNumberOfVerifiedRelationships(new RelationshipVerifiedBuilder().Build());

            if (numberOfVerifiedRelationships != 1)
            {
                return false;
            }

            return true;
        }
    }
}
