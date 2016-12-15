using System;
using System.Collections.Generic;
using Quartz;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public abstract class Poll<T, TOut> : IJob
    {
        protected Action<LogLevel, string> Log { get; set; }

        protected Func<PipelineResult<T>, PipelineResult<TOut>> Pipeline { get; set; }

        public abstract void Execute(IJobExecutionContext context);

        public Poll<T, TOut> SetLog(Action<LogLevel, string> log)
        {
            Log = log;
            return this;
        }

        public Poll<T, TOut> BuildPipeline(Func<PipelineResult<T>, PipelineResult<TOut>> pipeline)
        {
            Pipeline = pipeline;
            return this;
        }
    }

    public abstract class EntityListPoll<T,TOut> : Poll<T, TOut>
    {
        private Func<IEnumerable<T>> Source { get; set; }

        protected EntityListPoll()
        {
            Log = (level, s) => { };
            Source = () => new List<T>();
            Configure(this);
        }

        public abstract void Configure(EntityListPoll<T,TOut> cfg);
        
        public EntityListPoll<T, TOut> SetSource(Func<IEnumerable<T>> source)
        {
            Source = source;
            return this;
        }

        public override void Execute(IJobExecutionContext context)
        {
            foreach (var item in Source())
                Pipeline(item.Return(Log));
        }
    }

    public abstract class EntityPoll<T, TOut> : Poll<T, TOut>
    {
        private Func<T> Source { get; set; }
        
        protected EntityPoll()
        {
            Log = (level, s) => { };
            Configure(this);
            if (Source == null)
                throw new ArgumentException("Must configure source");
        }

        public abstract void Configure(EntityPoll<T, TOut> cfg);

        public EntityPoll<T, TOut> SetSource(Func<T> source)
        {
            Source = source;
            return this;
        }

        public override void Execute(IJobExecutionContext context)
        {
            Pipeline(Source().Return(Log));
        }
    }
}