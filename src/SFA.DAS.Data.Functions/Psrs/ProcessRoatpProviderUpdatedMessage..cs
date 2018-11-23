using System;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Sfa.Roatp.Events;
using SFA.DAS.Data.Functions.Extensions;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class ProcessRoatpProviderUpdatedMessage
    {
        [FunctionName("ProcessRoatpProviderUpdatedMessages")]
        [NServiceBusConfiguration("Sfa.Roatp.Events.RoatpProviderUpdated", typeof(RoatpProviderUpdated))]
        public static void Run([ServiceBusTrigger("sfa.roatp.events.roatpproviderupdated", AccessRights.Manage, Connection = "MessageBusConnectionString")]
            BrokeredMessage message,
            ExecutionContext executionContext,
            [Inject] ILog log)
        {
            try
            {
                var roatpProviderUpdatedEvent = message.DeserializeJsonMessage<RoatpProviderUpdated>();
                log.Info($"C# ServiceBus queue trigger function processed event: {roatpProviderUpdatedEvent}");
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Unable to deserialize message body for message queue sfa.roatp.events.roatpproviderupdated. messageId: {message.MessageId} {{ID={executionContext.InvocationId}}}");
                message.Defer();
            }
        }
    }
}
