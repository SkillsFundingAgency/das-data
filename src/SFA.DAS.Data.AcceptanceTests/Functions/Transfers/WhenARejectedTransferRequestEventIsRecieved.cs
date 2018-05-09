using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Transfers
{
    [TestFixture]
    public class WhenARejectedTransferRequestEventIsRecieved : TransferTestBase
    {

        [Test]
        public async Task ThenProcessandStoreMessage()
        {
            //Arrange
            await base.SetupDatabase();

            var message = new RejectedTransferConnectionInvitationEvent()
            {
                CreatedAt = DateTime.UtcNow,
                SenderAccountHashedId = "aabbcc16",
                SenderAccountId = 12345,
                RejectorUserId = 98765,
                ReceiverAccountId = 54321


            };
            var logger = new TraceWriterStub(TraceLevel.Verbose);

            var sentMessage = new WhenAStartTransferRequestEventIsRecieved();

            await sentMessage.ThenProcessandStoreMessage();

            //Act
            DAS.Data.Functions.Transfers.ProcessTransferRelationshipRejectedMessage.Run(message, null, logger, transferRelationshipService, log);

            //Assert
           // Assert.AreEqual(1, logger.Traces.Count);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);


        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var transferLAtestRejectedTransactionCount = await transferTestsRepository.GetNumberOfLatestRejectedTransferRelationships();
            if (transferLAtestRejectedTransactionCount != 1)
            {
                return false;
            }

            var transferSentTransactionCount = await transferTestsRepository.GetNumberOfSentTransferRelationships();
            if (transferSentTransactionCount != 1)
            {
                return false;
            }
            var transferRejectedTransactionCount = await transferTestsRepository.GetNumberOfRejectedTransferRelationships();
            if (transferRejectedTransactionCount != 1)
            {
                return false;
            }

            return true;
        }

    }
}
