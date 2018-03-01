using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SFA.DAS.Data.Functions
{
    public static class GetAccountStatisticsFunction
    {
        [FunctionName("GetAccountStatisticsFunction")]
        public static void Run([TimerTrigger("%CronSchedule%")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at GetAccountStatisticsFunction: {DateTime.Now}");
        }
    }
}
