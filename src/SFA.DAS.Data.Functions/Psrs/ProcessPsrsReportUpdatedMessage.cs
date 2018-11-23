using System;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Functions.Extensions;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class ProcessPsrsReportUpdatedMessage
    {
        [FunctionName("ProcessPsrsReportUpdatedMessages")]
        [NServiceBusConfiguration("SFA.DAS.PSRService.Messages.Events.ReportUpdated", typeof(ReportUpdated))]
        public static void Run([ServiceBusTrigger("sfa.das.psrservice.messages.events.reportupdated", AccessRights.Manage, Connection = "MessageBusConnectionString")]
            BrokeredMessage message,
            ExecutionContext executionContext,
            [Inject] ILog log)
        {
            try
            {
                var reportUpdatedEvent = message.DeserializeJsonMessage<ReportUpdated>();
                log.Info($"C# ServiceBus queue trigger function processed event: {reportUpdatedEvent}");
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Unable to deserialize message body for message queue sfa.das.psrservice.messages.events.reportupdated. messageId: {message.MessageId} {{ID={executionContext.InvocationId}}}");
                message.Defer();
            }
        }
    }
}
