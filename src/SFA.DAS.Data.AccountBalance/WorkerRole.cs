using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.AccountBalance
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("SFA.DAS.Data.AccountBalance is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("SFA.DAS.Data.AccountBalance has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SFA.DAS.Data.AccountBalance is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("SFA.DAS.Data.AccountBalance has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var configuration = new AccountApiConfiguration
            {
                ApiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"],
                ClientSecret = ConfigurationManager.AppSettings["ClientSecret"]
            };
            var client = new AccountApiClient(configuration);
            var source = new ApiWrapper(client);


            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}


//public override bool OnStart()
//{
//    ISchedulerFactory sf = new StdSchedulerFactory();
//    var sched = sf.GetScheduler();

//    LoadJobs(sched);

//    sched.Start();

//    return base.OnStart();
//}

//public static void LoadJobs(IScheduler sched)
//{
//    IJobDetail job = JobBuilder.Create()
//                    .WithIdentity("job1", "group1")
//                    .Build();

//    ITrigger trigger = TriggerBuilder.Create<DatabaseMaintenanceJob>()
//        .WithIdentity("trigger1", "group1")
//        .ForJob(job)
//        .StartNow()
//        .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInHours(1))
//        .Build();

//    sched.ScheduleJob(job, trigger);
//}

//class DatabaseMaintenanceJob : IJob
//{
//    public void Execute(IJobExecutionContext context)
//    {
//        // Code to do the db maintenance here
//    }
//}