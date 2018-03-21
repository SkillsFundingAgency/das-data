using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions
{
    public static class GetCommitmentStatisticsFunction
    {
        [FunctionName("GetCommitmentStatisticsFunction")]
        [return: Queue(QueueNames.ProviderQueueName)]
        public static async Task<CommitmentProcessingCompletedMessage> Run(
            [QueueTrigger(QueueNames.CommitmentsQueueName, Connection = "StorageConnectionString")] EasProcessingCompletedMessage message, 
            [Inject] ILog log,
            [Inject] IStatisticsService statsService)
        {
            log.Info("Gathering statics for the commitments area of the system");

            var returnMessage = await statsService.CollateCommitmentStatisticsMetrics();

            return returnMessage as CommitmentProcessingCompletedMessage;
        }
    }
}
