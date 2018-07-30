using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
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

namespace SFA.DAS.Data.Functions.Transfers
{
    public class ProcessTransferRelationshipRejectedMessage
    {
        [FunctionName("ProcessTransferRelationshipRejectedMessage")]
        public static void Run([ServiceBusTrigger("rejected_transfer_connection_invitation", "RDS_RejectedTransferConnectionInvitiationProcessor", AccessRights.Manage,Connection = "MessageBusConnectionString")] RejectedTransferConnectionInvitationEvent message, ExecutionContext executionContext, TraceWriter log, [Inject] ITransferRelationshipService transferRelationshipMessageService, [Inject] ILog logger)
        {

            transferRelationshipMessageService.SaveRejectedMessage(message);

            logger.Info($"C# service bus trigger function executed at ProcessTransferRelationshipStartMessage: {DateTime.Now}");
        }

        [FunctionName("ProcessTransferRelationshipRejectedMessageDLQ")]
        public static void RunDLQ([ServiceBusTrigger("rejected_transfer_connection_invitation", "RDS_RejectedTransferConnectionInvitiationProcessor/$DeadLetterQueue", AccessRights.Manage, Connection = "MessageBusConnectionString")] BrokeredMessage bMessage, ExecutionContext executionContext, TraceWriter log, [Inject] ITransferRelationshipService transferRelationshipMessageService, [Inject] ILog logger)
        {

            log.Info($"Processing messageId: {bMessage.MessageId} {{ID={executionContext.InvocationId}}}");

            RejectedTransferConnectionInvitationEvent messageBody = null;
            try
            {
                messageBody = bMessage.GetBody<RejectedTransferConnectionInvitationEvent>();
            }
            catch (Exception e)
            {
                log.Error($"Unable to deserialize message body for message queue sent_transfer_connection_invitation. messageId: {bMessage.MessageId} {{ID={executionContext.InvocationId}}}", e);
                bMessage.Defer();
            }

            if (messageBody != null)
            {
                try
                {
                    transferRelationshipMessageService.SaveRejectedMessage(messageBody);
                    log.Info($"Processing Completed for messageId: {bMessage.MessageId} {{ID={executionContext.InvocationId}}}");
                    bMessage.Complete();
                }
                catch (Exception e)
                {
                    log.Error($"Unable to save message for queue sent_transfer_connection_invitation DLQ. messageId: {bMessage.MessageId} {{ID={executionContext.InvocationId}}}", e);
                    bMessage.Defer();
                }

            }
        }


    }
}
