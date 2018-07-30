using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Commitments
{
    public static class ProcessCommitmentsRelationshipCreatedMessage
    {
        [FunctionName("ProcessCommitmentsRelationshipCreatedMessage")]
        public static void Run([ServiceBusTrigger("relationship_created", "RDS_RelationshipCreated", AccessRights.Manage, Connection = "MessageBusConnectionString")] RelationshipCreated message, ExecutionContext executionContext, TraceWriter log, [Inject] ICommitmentsRelationshipService commitmentsRelationshipService, [Inject] ILog logger )
        {
            commitmentsRelationshipService.SaveCreatedRelationship(message);

            logger.Info($"Service bus function ProcessCommitmentsRelationshipCreatedMessage executed at {DateTime.Now}");
        }
    }
}
