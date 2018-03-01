using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Functions.Ioc;

namespace SFA.DAS.Data.Functions
{
    public static class Function1
    {
        //[FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log, [Inject]ITestClass testClass)
        {
            log.Info($"C# Timer trigger function for Function1 executed at: {DateTime.Now}");
            testClass.Write();
        }
    }
}
