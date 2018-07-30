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
    public class WhenARelationshipCreatedMessageIsReceived : CommitmentsTestBase
    {
        [Test]
        public async Task ThenSaveRelationshipCreatedMessage()
        {
            await SetupDatabase();

            var relationship = new CommitmentsRelationshipBuilder().Build(true);

            var message = new RelationshipCreated {Relationship = relationship};

            var logger = new TraceWriterStub(TraceLevel.Verbose);

            DAS.Data.Functions.Commitments.ProcessCommitmentsRelationshipCreatedMessage.Run(message, null, logger, CommitmentsRelationshipService, Log);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var commitmentCount = await CommitmentsTestRepository.GetNumberOfLatestCommitmentsRelationships();
            return commitmentCount == 1;
        }
    }
}
