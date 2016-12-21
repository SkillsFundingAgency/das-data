using System;
using System.Collections.Generic;
using Quartz;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public abstract class Poll<T, TOut> : IJob
    {
        protected Poll()
        {
            Log = (level, s) => { };
        }

        protected Action<LoggingLevel, string> Log { get; private set; }

        protected Func<PipelineResult<T>, PipelineResult<TOut>> Pipeline { get; private set; }

        public abstract void Execute(IJobExecutionContext context);

        public Poll<T, TOut> SetLog(Action<LoggingLevel, string> log)
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

    public abstract class EntityListPoll<T, TOut> : Poll<T, TOut>
    {
        private Func<IEnumerable<T>> Source { get; set; }

        protected EntityListPoll()
        {
            Source = () => new List<T>();
            Configure(this);
        }

        public abstract void Configure(EntityListPoll<T, TOut> cfg);

        public EntityListPoll<T, TOut> SetSource(Func<IEnumerable<T>> source)
        {
            Source = source;
            return this;
        }

        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                foreach (var item in Source())
                    Pipeline(item.Return(Log));
            }
            catch (AggregateException ax)
            {
                foreach (var innerException in ax.InnerExceptions)
                    Log(LoggingLevel.Error, 
                        innerException.Message + ":" + innerException.StackTrace);
                throw;
            }
            catch (Exception e)
            {
                Log(LoggingLevel.Error, e.StackTrace);
                throw;
            }
            
        }
    }

    public abstract class EntityPoll<T, TOut> : Poll<T, TOut>
    {
        private Func<T> Source { get; set; }
        
        protected EntityPoll()
        {
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