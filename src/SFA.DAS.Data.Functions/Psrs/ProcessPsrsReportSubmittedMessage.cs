using System;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Functions.Extensions;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class ProcessPsrsReportSubmittedMessage
    {
        [FunctionName("ProcessPsrsReportSubmittedMessage")]
        public static void Run([ServiceBusTrigger("sfa.das.psrservice.messages.events.reportsubmitted", AccessRights.Manage, Connection = "MessageBusConnectionString")]
            BrokeredMessage message,
            ExecutionContext executionContext,
            [Inject] ILog log)
        {
            //TODO: Change direct reference to SFA.DAS.PSRService.Messages.dll to NuGet package.
            try
            {
                var reportSubmittedEvent = message.DeserializeJsonMessage<ReportSubmitted>();
                log.Info($"C# ServiceBus queue trigger function processed event: {reportSubmittedEvent}");
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Unable to deserialize message body for message queue sfa.das.reportsubmitted.messages.events.reportcreated. messageId: {message.MessageId} {{ID={executionContext.InvocationId}}}");
                message.Defer();
            }
        }
    }
}
