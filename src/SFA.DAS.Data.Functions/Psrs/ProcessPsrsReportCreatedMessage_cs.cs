using System;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Functions.Extensions;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class ProcessPsrsReportCreatedMessage
    {
        [FunctionName("ProcessPsrsReportCreatedMessages")]
        [NServiceBusConfiguration("SFA.DAS.PSRService.Messages.Events.ReportCreated", typeof(ReportCreated))]
        public static void Run([ServiceBusTrigger("sfa.das.psrservice.messages.events.reportcreated", AccessRights.Manage, Connection = "MessageBusConnectionString")]
            BrokeredMessage message,
            ExecutionContext executionContext,
            [Inject] ILog log)
        {
            try
            {
                var reportCreatedEvent = message.DeserializeJsonMessage<ReportCreated>();
                log.Info($"C# ServiceBus queue trigger function processed event: {reportCreatedEvent}");
                message.Complete();
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Unable to deserialize message body for message queue sfa.das.psrservice.messages.events.reportcreated. messageId: {message.MessageId} {{ID={executionContext.InvocationId}}}");
                message.Defer();
            }
        }
    }
}
