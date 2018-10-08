using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Psrs
{
    public static class CreatePsrsSubmittedReportFunction
    {
        [FunctionName("CreatePsrsSubmittedReports")]
        public static async Task Run([TimerTrigger("%CronSchedule%")] TimerInfo myTimer, [Inject] ILog log,
            [Inject] IPsrsReportsService psrsService)
        {
            await psrsService.CreatePsrsSubmittedReports();
        }

        [FunctionName("CreatePreviousSubmittedReports")]
        public static async Task<HttpResponseMessage> RunHttp(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = null)] HttpRequestMessage req, [Inject] ILog log,
            [Inject] IPsrsReportsService psrsService)
        {
            await psrsService.CreatePsrsSubmittedReports(DateTime.Now.Subtract(DateTime.Parse("2018-01-01")));
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
