//using System.Threading.Tasks;
//using Microsoft.Azure.WebJobs;
//using SFA.DAS.Data.Domain.Interfaces;
//using SFA.DAS.Data.Functions.Ioc;
//using SFA.DAS.Data.Functions.Statistics.Commands;
//using SFA.DAS.NLog.Logger;

//namespace SFA.DAS.Data.Functions.Statistics
//{
//    public static class GetPaymentsStatisticsFunction
//    {
//        [FunctionName("GetPaymentsStatisticsFunction")]
//        [Disable]
//        public static async Task Run([QueueTrigger(
//            QueueNames.ProviderQueueName, Connection = "StorageConnectionString")] CommitmentProcessingCompletedMessage message,
//            [Inject] ILog log,
//            [Inject] IStatisticsService statsService)
//        {
//            log.Info("Gathering statics for the payments area of the system");

//            await statsService.CollatePaymentStatisticsMetrics();
//        }
//    }
//}
