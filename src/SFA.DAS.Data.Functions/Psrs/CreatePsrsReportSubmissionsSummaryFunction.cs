using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Statistics
{
    public static class CreatePsrsReportSubmissionsSummaryFunction
    {
        [FunctionName("CreatePsrsReportSubmissionsSummary")]
        
        public static async Task Run([TimerTrigger("%CronSchedule%")] TimerInfo myTimer, [Inject] ILog log,
            [Inject] IPsrsReportsService psrsService)
        {

            psrsService.CreatePsrsReportSubmissionsSummary();


        }
    }
}
