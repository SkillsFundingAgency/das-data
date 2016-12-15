using System;
using System.Collections.Generic;
using Quartz;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public abstract class EventListPoll<T,TOut> : IJob
    {
        private Action<LogLevel, string> Log { get; set; }

        private Func<IEnumerable<T>> Source { get; set; }

        private Func<PipelineResult<T>, PipelineResult<TOut>> Pipeline { get; set; }

        protected EventListPoll()
        {
            Log = (level, s) => { };
            Source = () => new List<T>();
            Configure(this);
        }

        public abstract void Configure(EventListPoll<T,TOut> cfg);

        public EventListPoll<T, TOut> SetLog(Action<LogLevel, string> log)
        {
            Log = log;
            return this;
        }

        public EventListPoll<T, TOut> BuildPipeline(Func<PipelineResult<T>, PipelineResult<TOut>> pipeline)
        {
            Pipeline = pipeline;
            return this;
        }

        public EventListPoll<T, TOut> SetSource(Func<IEnumerable<T>> source)
        {
            Source = source;
            return this;
        }

        public void Execute(IJobExecutionContext context)
        {
            foreach (var item in Source())
                Pipeline(item.Return(Log));
        }
    }

    
    public static class Poll
    {
        //some timing stuff with quartz
        //will expect a function that returns an enumerable

        //some sequence thing that can shove multiple events through a pipeline
        //probably try to log as we go instead of all at the end
        //essentially a loop with yield, do something sensible with results


        

        public class PollWorker<T,TOut>
        {
            public Action<LogLevel,string> Log { get; set; }
            public string Frequency { get; set; } //default value
            public Func<IEnumerable<T>> Source { get; set; }
            public Func<PipelineResult<T>,PipelineResult<TOut>> Pipeline { get; set; }

            public PollWorker()
            {
                Log = (level, s) => { };
            }
        }


        public static void Every(string frequency)
        {
            
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
    }
}