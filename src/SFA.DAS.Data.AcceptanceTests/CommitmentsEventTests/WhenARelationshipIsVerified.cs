using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.AcceptanceTests.CommitmentsEventTests
{
    [TestFixture]
    public class WhenARelationshipIsVerified : CommitmentsEventTestBase
    {
        private static readonly RelationshipVerifiedBuilder RelationshipBuilder = new RelationshipVerifiedBuilder();
        private readonly RelationshipVerified _relationshipVerified = RelationshipBuilder.Build();
        private Relationship Relationship => new Relationship
        {
            EmployerAccountId = _relationshipVerified.EmployerAccountId,
            LegalEntityAddress = RelationshipBuilder.LegalEntityAddress,
            LegalEntityId = _relationshipVerified.LegalEntityId,
            LegalEntityName = RelationshipBuilder.LegalEntityName,
            LegalEntityOrganisationType = RelationshipBuilder.LegalOrganisationType,
            ProviderId = _relationshipVerified.ProviderId,
            ProviderName = RelationshipBuilder.ProviderName,
            Verified = false
        };

        [Test]
        public void ThenTheRelationshipIsVerified()
        {
            InsertRowToUpdate();
            AzureTopicMessageBus.PublishAsync(_relationshipVerified);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private void InsertRowToUpdate()
        {
            EventTestsRepository.InsertRelationship(Relationship).Wait();
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var numberOfVerifiedRelationships =
                await EventTestsRepository.GetNumberOfVerifiedRelationships(_relationshipVerified);

            if (numberOfVerifiedRelationships != 1)
            {
                return false;
            }

            return true;
        }
    }
}
