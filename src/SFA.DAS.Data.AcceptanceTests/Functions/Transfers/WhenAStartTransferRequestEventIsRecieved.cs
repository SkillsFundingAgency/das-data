using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;
using Microsoft.Azure.WebJobs.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Transfers
{
    [TestFixture]
    public class WhenAStartTransferRequestEventIsRecieved
    {

        [Test]
  
      public void ThenProcessandStoreMessage()
        {



            var message = new SentTransferConnectionInvitationEvent()
            {
                CreatedAt = DateTime.UtcNow,
                SenderAccountHashedId = "aabbcc16",
                SenderAccountId = 12345,
                SentByUserId = 98765,
                ReceiverAccountId = 54321

            
            };
            var logger = new TraceWriterStub(TraceLevel.Verbose);
           

             DAS.Data.Functions.Transfers.ProcessTransferRelationshipSentMessage.Run(message,null, logger);

            Assert.AreEqual(1, logger.Traces.Count);

            
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
