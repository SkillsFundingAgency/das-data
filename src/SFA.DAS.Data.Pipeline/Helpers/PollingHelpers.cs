using System;
using System.Collections.Generic;
using Quartz;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public abstract class EntityListPoll<T,TOut> : IJob
    {
        private Action<LogLevel, string> Log { get; set; }

        private Func<IEnumerable<T>> Source { get; set; }

        private Func<PipelineResult<T>, PipelineResult<TOut>> Pipeline { get; set; }

        protected EntityListPoll()
        {
            Log = (level, s) => { };
            Source = () => new List<T>();
            Configure(this);
        }

        public abstract void Configure(EntityListPoll<T,TOut> cfg);

        public EntityListPoll<T, TOut> SetLog(Action<LogLevel, string> log)
        {
            Log = log;
            return this;
        }

        public EntityListPoll<T, TOut> BuildPipeline(Func<PipelineResult<T>, PipelineResult<TOut>> pipeline)
        {
            Pipeline = pipeline;
            return this;
        }

        public EntityListPoll<T, TOut> SetSource(Func<IEnumerable<T>> source)
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
}