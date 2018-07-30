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
    public static class ProcessCommitmentsRelationshipVerifiedMessage
    {
        [FunctionName("ProcessCommitmentsRelationshipVerifiedMessage")]
        public static void Run([ServiceBusTrigger("relationship_verified", "RDS_RelationshipVerified", AccessRights.Manage, Connection = "MessageBusConnectionString")] RelationshipVerified message, ExecutionContext executionContext, TraceWriter log, [Inject] ICommitmentsRelationshipService commitmentsRelationshipService, [Inject] ILog logger )
        {
            commitmentsRelationshipService.SaveVerifiedRelationship(message);

            logger.Info($"Service bus function ProcessCommitmentsRelationshipVerifiedMessage executed at {DateTime.Now}");
        }
    }
}
