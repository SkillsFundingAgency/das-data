using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Quartz;
using Quartz.Impl;
using SFA.DAS.Data.Pipeline;
using SFA.DAS.Data.Pipeline.Helpers;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;
using Simple.Data;

namespace SFA.DAS.Data.AccountBalance
{
    public class AccountBalanceJob : EntityListPoll<AccountWithBalanceViewModel, AccountBalanceJob.ToStore>
    {
        public class ToStore : TableEntity
        {
            public ToStore()
            {
                PartitionKey = "test";
                RowKey = Guid.NewGuid().ToString();
            }

            public ToStore(AccountWithBalanceViewModel model)
            {
                PartitionKey = "test";
                RowKey = Guid.NewGuid().ToString();
                AccountName = model.AccountName;
                AccountHashId = model.AccountHashId;
                AccountId = model.AccountId;
                Balance = model.Balance;
            }

            public string AccountName { get; set; }
            public string AccountHashId { get; set; }
            public long AccountId { get; set; }
            public decimal Balance { get; set; }
        }

        public void DumbLog(LoggingLevel level, string message)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("Storage"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("test");
            table.CreateIfNotExists();
            var insertOperation =
                TableOperation.Insert(
                    new AccountBalanceJob.ToStore(new AccountWithBalanceViewModel { AccountName = message }));
            table.Execute(insertOperation);
        }

        public override void Configure(EntityListPoll<AccountWithBalanceViewModel, AccountBalanceJob.ToStore> cfg)
        {
            DumbLog(LoggingLevel.Info, "Configure");

            var configuration = new AccountApiConfiguration
            {
                ApiBaseUrl = CloudConfigurationManager.GetSetting("AccountApi.ClientSecret"),
                ClientSecret = CloudConfigurationManager.GetSetting("AccountApi.ClientSecret"),
                ClientId = CloudConfigurationManager.GetSetting("AccountApi.ClientId"),
                IdentifierUri = CloudConfigurationManager.GetSetting("AccountApi.IdentifierUri"),
                Tenant = CloudConfigurationManager.GetSetting("AccountApi.Tenant")
            };
            var client = new AccountApiClient(configuration);
            var source = new ApiWrapper(client);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("Storage"));

            //var db = Database.OpenConnection(CloudConfigurationManager.GetSetting("StagingConnectionString"));
            //var conn = new DbWrapper {Wrapper = db};

            //no need to modify the view model
            cfg.SetSource(source.GetAccounts)
                .SetLog(DumbLog)
                .BuildPipeline(v =>
                    v.Step(i => Result.Win(new ToStore(i), "converted to table entity"))
                        .Store(storageAccount, "balance"));

            DumbLog(LoggingLevel.Info, "Configure Done");
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
            Trace.TraceInformation("SFA.DAS.Data.AccountBalance is starting");
            
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
                .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInMinutes(5))
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