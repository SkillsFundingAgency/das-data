using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Transfers
{
    [TestFixture]
    public class WhenAStartTransferRequestEventIsRecieved: TransferTestBase
    {

        [Test]
  
      public async Task ThenProcessandStoreMessage()
        {

            await base.SetupDatabase();

            var message = new SentTransferConnectionInvitationEvent()
            {
                CreatedAt = DateTime.UtcNow,
                SenderAccountHashedId = "aabbcc16",
                SenderAccountId = 12345,
                SentByUserId = 98765,
                ReceiverAccountId = 54321

            
            };
            var logger = new TraceWriterStub(TraceLevel.Verbose);
           

             DAS.Data.Functions.Transfers.ProcessTransferRelationshipSentMessage.Run(message,null, logger, transferRelationshipService, log);

          //  Assert.AreEqual(1, logger.Traces.Count);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);


        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var transferTransactionCount = await transferTestsRepository.GetNumberOfSentTransferRelationships();
            if (transferTransactionCount != 1)
            {
                return false;
            }

            return true;
        }

    }
    public class TraceWriterStub : TraceWriter
    {
        protected TraceLevel _level;
        protected List<TraceEvent> _traces;

        public TraceWriterStub(TraceLevel level) : base(level)
        {
            _level = level;
            _traces = new List<TraceEvent>();
        }

        public override void Trace(TraceEvent traceEvent)
        {
            _traces.Add(traceEvent);
        }

        public List<TraceEvent> Traces => _traces;
    }
}
