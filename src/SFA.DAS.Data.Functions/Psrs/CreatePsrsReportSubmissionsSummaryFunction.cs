using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class CreatePsrsReportSubmissionsSummaryFunction
    {
        [Disable]
        [FunctionName("CreatePsrsReportSubmissionsSummary")]
        public static async Task Run([TimerTrigger("%CronSchedule%")] TimerInfo myTimer, [Inject] ILog log,
            [Inject] IPsrsReportsService psrsService)
        {
            await psrsService.CreatePsrsReportSubmissionsSummary();
        }
    }
}
