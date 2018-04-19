using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Enum;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Data.Functions.Transfers
{
    public class ProcessTransferRelationshipApprovedMessage
    {
        [FunctionName("ProcessTransferRelationshipApprovedMessage")]
        public static void Run([ServiceBusTrigger("approved_transfer_connection_invitation", "RDS_ApprovedTransferConnectionInvitiationProcessor", AccessRights.Manage,Connection = "MessageBusConnectionString")] ApprovedTransferConnectionInvitationEvent message, ExecutionContext executionContext, TraceWriter log)
        {

            //This needs to be pulled from DI rather than being created here.
            var transferRelationshipService = new TransferRelationshipMessageService();


            transferRelationshipService.SaveApprovedMessage(message);

            log.Info($"C# service bus trigger function executed at ProcessTransferRelationshipStartMessage: {DateTime.Now}");
        }
        
    }
}
