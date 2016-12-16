using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Quartz;
using Quartz.Impl;
using SFA.DAS.Data.Pipeline.Helpers;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;
using Simple.Data;

namespace SFA.DAS.Data.AccountBalance
{
    public class AccountBalanceJob : EntityListPoll<AccountWithBalanceViewModel, AccountWithBalanceViewModel>
    {
        public override void Configure(EntityListPoll<AccountWithBalanceViewModel, AccountWithBalanceViewModel> cfg)
        {
            var configuration = new AccountApiConfiguration
            {
                ApiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"],
                ClientSecret = ConfigurationManager.AppSettings["ClientSecret"]
            };
            var client = new AccountApiClient(configuration);
            var source = new ApiWrapper(client);
            
            var db = Database.OpenNamedConnection("staging");
            var conn = new DbWrapper {Wrapper = db};

            //no need to modify the view model
            cfg.SetSource(source.GetAccounts)
                .SetLog(Logging.Log)
                .BuildPipeline(v => 
                    v.Store(conn, "balance"));
        }
    }

    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IScheduler sched;

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
            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();

            LoadJobs(sched);

            sched.Start();

            return base.OnStart();
        }

        public static void LoadJobs(IScheduler sched)
        {
            var job = JobBuilder.Create<AccountBalanceJob>()
                .WithIdentity("job1", "group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInHours(1))
                .Build();

            sched.ScheduleJob(job, trigger);
        }

        public override void OnStop()
        {
            sched.Shutdown(false);
            sched = null;
            base.OnStop();
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(10000);
            }
        }
    }
}