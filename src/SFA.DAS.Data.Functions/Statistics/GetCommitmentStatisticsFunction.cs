using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.Data.Application.Messages;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Statistics
{
    public static class GetCommitmentStatisticsFunction
    {
        [FunctionName("GetCommitmentStatisticsFunction")]
        [Disable]
       // [Disable]
        public static async Task Run([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer, [Inject] ILog log,
        [Inject] IStatisticsService statsService)
        {
            log.Info("Gathering statistics for the commitments area of the system");

            var returnMessage = await statsService.CollateCommitmentStatisticsMetrics();
       }
    }
}
