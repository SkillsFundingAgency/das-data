using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Statistics
{
    public static class GetPaymentsStatisticsFunction
    {
        [FunctionName("GetPaymentsStatisticsFunction")]
       // [Disable]
        public static async Task Run([TimerTrigger("%CronSchedule%")] TimerInfo myTimer, TraceWriter traceLog, [Inject] ILog log,
        [Inject] IStatisticsService statsService)
        {
            log.Info("Gathering statics for the payments area of the system");

            try
            {
                await statsService.CollatePaymentStatisticsMetrics();
            }
            catch (Exception e)
            {
                traceLog.Error($"Error processing GetPaymentsStatisticsFunction to pull data into RDS: {e.Message}", e);
                throw;
            }
        }
    }
}
