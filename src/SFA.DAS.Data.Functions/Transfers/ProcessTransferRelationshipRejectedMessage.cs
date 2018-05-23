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

     
    }
}