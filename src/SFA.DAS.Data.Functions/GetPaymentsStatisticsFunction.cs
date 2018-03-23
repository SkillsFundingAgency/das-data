using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions
{
    public static class GetPaymentsStatisticsFunction
    {
        [FunctionName("GetPaymentsStatisticsFunction")]
        public static async Task Run([QueueTrigger(
            QueueNames.ProviderQueueName, Connection = "StorageConnectionString")] CommitmentProcessingCompletedMessage message, 
            [Inject] ILog log,
            [Inject] IStatisticsService statsService)
        {
            log.Info("Gathering statics for the payments area of the system");

            await statsService.CollatePaymentStatisticsMetrics();
        }
    }
}