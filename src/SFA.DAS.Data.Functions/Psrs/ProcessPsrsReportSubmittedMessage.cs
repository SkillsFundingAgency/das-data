using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Functions.Extensions;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class ProcessPsrsReportSubmittedMessage
    {
        [FunctionName("ProcessPsrsReportSubmittedMessage")]
        public static void Run([ServiceBusTrigger("sfa.das.psrservice.messages.events.reportsubmitted",
                AccessRights.Manage, Connection = "MessageBusConnectionString")]
            BrokeredMessage message,
            ExecutionContext executionContext,
            [Inject] ILog log)
        {
            try
            {
                var reportSubmittedEvent = message.DeserializeJsonMessage<ReportSubmitted>();
                log.Info($"C# ServiceBus queue trigger function processed event: {reportSubmittedEvent}");
                message.Complete();
            }
            catch (Exception ex)
            {
                log.Error(ex,
                    $"Unable to deserialize message body for message queue sfa.das.reportsubmitted.messages.events.reportcreated. messageId: {message.MessageId} {{ID={executionContext.InvocationId}}}");
                message.Defer();
            }
        }

        [FunctionName("ProcessPsrsReportSubmittedMessage_TEST")]
        //[Disable]
        public static void RunTEST([ServiceBusTrigger("sfa_test_transfer_connection_invitation", "RDS_Psrs_Test_Sub",
                AccessRights.Manage, Connection = "MessageBusConnectionString")]
            BrokeredMessage bMessage, ExecutionContext executionContext, TraceWriter log,
            [Inject] ITransferRelationshipService transferRelationshipMessageService, [Inject] ILog logger)
        {
        }
    }
}
