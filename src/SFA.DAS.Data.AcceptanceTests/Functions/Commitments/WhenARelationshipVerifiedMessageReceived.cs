using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.AcceptanceTests.Functions.Transfers;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Commitments
{
    [TestFixture]
    public class WhenARelationshipVerifiedMessageReceived : CommitmentsTestBase
    {
        [Test]
        public async Task ThenSaveRelationshipVerifiedMessage()
        {
            await SetupDatabase();

            var relationship = new CommitmentsRelationshipBuilder().Build(false);

            await InsertCommitmentsRelationship(relationship);

            var message = new RelationshipVerified
            {
                EmployerAccountId = relationship.EmployerAccountId,
                LegalEntityId = relationship.LegalEntityId,
                ProviderId = relationship.ProviderId,
                Verified = true
            };

            var logger = new TraceWriterStub(TraceLevel.Verbose);

            DAS.Data.Functions.Commitments.ProcessCommitmentsRelationshipVerifiedMessage.Run(message, null, logger, CommitmentsRelationshipService, Log);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var verifiedRelationshipCount = await CommitmentsTestRepository.GetNumberOfLatestVerifiedCommitmentsRelationships();
            return verifiedRelationshipCount == 1;
        }
    }
}
