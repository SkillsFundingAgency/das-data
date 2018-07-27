using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Enum;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.NLog.Logger;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace SFA.DAS.Data.Functions.Transfers
{
    public class ProcessTransferRelationshipSentMessage
    {
        [FunctionName("ProcessTransferRelationshipSentMessage")]
        public static void Run([ServiceBusTrigger("sent_transfer_connection_invitation", "RDS_SentTransferConnectionInvitiationProcessor", AccessRights.Manage,Connection = "MessageBusConnectionString")] SentTransferConnectionInvitationEvent message, ExecutionContext executionContext, TraceWriter log, [Inject] ITransferRelationshipService transferRelationshipMessageService, [Inject] ILog logger)
        {


            transferRelationshipMessageService.SaveSentMessage(message);

            logger.Info($"C# service bus trigger function executed at ProcessTransferRelationshipStartMessage: {DateTime.Now}");
        }

        [FunctionName("ProcessTransferRelationshipSentMessageDLQ")]
        [Disable]
        public static void RunDLQ([ServiceBusTrigger("sent_transfer_connection_invitation", "RDS_SentTransferConnectionInvitiationProcessor/$DeadLetterQueue", AccessRights.Manage, Connection = "MessageBusConnectionString")] BrokeredMessage bMessage, ExecutionContext executionContext, TraceWriter log, [Inject] ITransferRelationshipService transferRelationshipMessageService, [Inject] ILog logger)
        {

            SentTransferConnectionInvitationEvent messageBody = null;
            try
            {
                messageBody = bMessage.GetBody<SentTransferConnectionInvitationEvent>();
            }
            catch (Exception e)
            {
                logger.Error(e, "Unable to deserialize message body for message queue sent_transfer_connection_invitation");
                bMessage.Defer();
            }

            if (messageBody != null)
            {
                transferRelationshipMessageService.SaveSentMessage(messageBody);
                bMessage.Complete();
            }
      
        }


    }
}
