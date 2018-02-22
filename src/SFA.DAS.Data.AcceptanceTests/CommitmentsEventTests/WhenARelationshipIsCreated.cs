using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.AcceptanceTests.CommitmentsEventTests
{
    [TestFixture]
    public class WhenARelationshipIsCreated : CommitmentsEventTestBase
    {
        [Test]
        public void ThenTheRelationshipDetailsAreStored()
        {
            AzureTopicMessageBus.PublishAsync(new RelationshipCreatedBuilder().Build());

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var numberOfProviders = await EventTestsRepository.GetNumberOfRelationships();
            if (numberOfProviders != 1)
            {
                return false;
            }

            return true;
        }
    }
}
