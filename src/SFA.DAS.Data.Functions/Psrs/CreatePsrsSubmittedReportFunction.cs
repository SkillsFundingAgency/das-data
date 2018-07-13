using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Statistics
{
    public static class CreatePsrsSubmittedReportFunction
    {
        [FunctionName("CreatePsrsSubmittedReports")]
        public static async Task Run([TimerTrigger("00:00:15")] TimerInfo myTimer, [Inject] ILog log,
            [Inject] IPsrsReportsService psrsService)
        {
            var timeSpan = TimeSpan.Parse(myTimer.Schedule.ToString().Replace("Constant: ",""));
            await psrsService.CreatePsrsSubmittedReports(timeSpan);


        }
    }
}
