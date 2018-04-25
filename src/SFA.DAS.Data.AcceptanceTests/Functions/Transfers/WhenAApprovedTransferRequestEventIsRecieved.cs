using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Transfers
{
    [TestFixture]
    public class WhenAApprovedTransferRequestEventIsRecieved : TransferTestBase
    {
 
        [Test]
      public async Task ThenProcessandStoreMessage()
        {
            //Arrange
            await base.SetupDatabase();

            var message = new ApprovedTransferConnectionInvitationEvent()
            {
                CreatedAt = DateTime.UtcNow,
                SenderAccountHashedId = "aabbcc16",
                SenderAccountId = 12345,
                ApprovedByUserId = 98765,
                ReceiverAccountId = 54321

            
            };

            var sentMessage = new WhenAStartTransferRequestEventIsRecieved();

            await sentMessage.ThenProcessandStoreMessage();

            var logger = new TraceWriterStub(TraceLevel.Verbose);
           
            //Act
             DAS.Data.Functions.Transfers.ProcessTransferRelationshipApprovedMessage.Run(message,null, logger, transferRelationshipService, log);


            //Assert
            //Assert.AreEqual(1, logger.Traces.Count);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);


        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            //var transferLatestApprovedTransactionCount = await transferTestsRepository.GetNumberOfLatestApprovedTransferRelationships();
            //if (transferLatestApprovedTransactionCount != 1)
            //{
            //    return false;
            //}

            var transferSentTransactionCount = await transferTestsRepository.GetNumberOfSentTransferRelationships();
            if (transferSentTransactionCount != 1)
            {
                return false;
            }
            var transferApprovedTransactionCount = await transferTestsRepository.GetNumberOfApprovedTransferRelationships();
            if (transferApprovedTransactionCount != 1)
            {
                return false;
            }

            return true;

      
        }

    }
    
}
