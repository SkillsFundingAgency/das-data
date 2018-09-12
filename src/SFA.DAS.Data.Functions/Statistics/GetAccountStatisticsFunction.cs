using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Statistics
{
    public static class GetAccountStatisticsFunction
    {
        [FunctionName("GetAccountStatisticsFunction")]
        public static async Task Run([TimerTrigger("%CronSchedule%")] TimerInfo myTimer, TraceWriter traceLog, [Inject] ILog log,
            [Inject] IStatisticsService statsService)
        {
            log.Debug("Gathering statistics for the EAS area of the system");

            try
            {
                var message = await statsService.CollateEasMetrics();
            }
            catch (Exception e)
            {
                traceLog.Error($"Error processing GetAccountStatisticsFunction to pull data into RDS: {e.Message}", e);
            }
        }
    }
}
