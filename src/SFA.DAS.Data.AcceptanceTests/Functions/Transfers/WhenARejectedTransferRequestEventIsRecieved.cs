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
    public class WhenARejectedTransferRequestEventIsRecieved
    {

        [Test]

        public void ThenProcessandStoreMessage()
        {



            var message = new RejectedTransferConnectionInvitationEvent()
            {
                CreatedAt = DateTime.UtcNow,
                SenderAccountHashedId = "aabbcc16",
                SenderAccountId = 12345,
                RejectorUserId = 98765,
                ReceiverAccountId = 54321


            };
            var logger = new TraceWriterStub(TraceLevel.Verbose);


            DAS.Data.Functions.Transfers.ProcessTransferRelationshipRejectedMessage.Run(message, null, logger);

            Assert.AreEqual(1, logger.Traces.Count);


        }

    }
}
