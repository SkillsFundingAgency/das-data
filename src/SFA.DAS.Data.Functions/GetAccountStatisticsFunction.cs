using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions
{
    public static class GetAccountStatisticsFunction
    {
        [FunctionName("GetAccountStatisticsFunction")]
        public static void Run([TimerTrigger("%CronSchedule%")]TimerInfo myTimer, TraceWriter log, [Inject] ILog logger )
        {
            log.Info($"C# Timer trigger function executed at GetAccountStatisticsFunction: {DateTime.Now}");

            logger.Debug("gathering statistics for the EAS area of the system");
        }
    }
}
