//using System;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.ServiceBus.Messaging;

//namespace SFA.DAS.Data.Functions
//{
//    public class ProcessTransferRelationshipStartMessage
//    {
//        [FunctionName("ProcessTransferRelationshipStartMessage")]
//        [Disable]
//        public static void Run([ServiceBusTrigger("transfer_connection_invitation_sent", "RDS_TransferConnectionInvitationSentProcessor", AccessRights.Manage,Connection = "MessageBusConnectionString")]string paymentCreatedMessage, ExecutionContext executionContext, TraceWriter log)
//        {
//            log.Info($"C# service bus trigger function executed at ProcessTransferRelationshipStartMessage: {DateTime.Now}");
//        }
//    }
//}
