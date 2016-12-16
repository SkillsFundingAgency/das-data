using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Data.Pipeline
{
    public static class ResultExtensions
    {
        public static Success<T> Return<T>(
            this T instance, Action<LoggingLevel,string> log = null)
            => new Success<T>(instance, "",log);

        public static Success<T> Return<T>(
            this T instance, Action rolback, Action<LoggingLevel, string> log = null)
            => new Success<T>(instance, "", rolback,log);
    }

    public class Result
    {
        public static Failure<T> Fail<T>(string message)
        {
            return new Failure<T>(message);
        }

        public static Success<T> Win<T>(T result, string message)
        {
            return new Success<T>(result, message);
        }
    }

    public abstract class PipelineResult<T>
    {
        protected string Message;
        protected List<Action> Rollbacks = new List<Action>();
        protected Action<LoggingLevel, string> Log = ((level, s) => { });
        
        private TRes Error<TRes>(TRes obj)
        {
            Log(LoggingLevel.Error, obj.ToString());
            return obj;
        }

        private TRes Info<TRes>(TRes obj)
        {
            Log(LoggingLevel.Info, obj.ToString());
            return obj;
        }

        protected T _instance;
        public PipelineResult<TO> Step<TO>(Func<T, PipelineResult<TO>> func)
        {
            if (!IsSuccess())
                return new Failure<TO>(Message,Log);
            try
            {
                var result = func(_instance);
                
                Rollbacks.AddRange(result.Rollbacks);
                result.Rollbacks = Rollbacks;
                result.Log = Log;

                if (result is Success<TO>)
                    return Info(result);
                
                foreach (var rollback in Rollbacks)
                    rollback();

                return Error(result);
            }
            catch (Exception ex)
            {
                return Error(new ExceptionFailure<TO>(ex, Log));
            }
        }

        public PipelineResult<TO> Step<TO>(Func<T, PipelineResult<TO>> func, Action rollback)
        {
            Rollbacks.Add(rollback);
            return Step(func);
        }

        public abstract bool IsSuccess();

        public virtual T Content => _instance;
    }

    public class Success<T> : PipelineResult<T>
    {
        public Success(T instance, string message, Action<LoggingLevel, string> log = null)
        {
            Log = log ?? ((level, s) => { });
            Message = message;
            _instance = instance;
        }

        public Success(
            T instance, string message, Action rollback, Action<LoggingLevel,string> log = null) : this(instance, message)
        {
            Log = log ?? ((level, s) => { });
            Rollbacks.Add(rollback);
        }

        public override string ToString()
        {
            return "Success: " + Message;
        }

        public override bool IsSuccess() => true;
    }

    public class Failure<T> : PipelineResult<T>
    {
        public Failure(string message, Action<LoggingLevel, string> log = null)
        {
            Log = log ?? ((level, s) => { });
            Message = message;
        }
        
        public override string ToString()
        {
            return "Failure: " + Message;
        }

        public override bool IsSuccess() => false;
    }

    public class ExceptionFailure<T> : Failure<T>
    {
        public ExceptionFailure(string message, Action<LoggingLevel, string> log = null) : base(message)
        {
            Log = log ?? ((level, s) => { });
        }

        public ExceptionFailure(Exception e, Action<LoggingLevel, string> log = null) : this(e.Message, log)
        {
            Log = log ?? ((level, s) => { });
        }

        public override string ToString()
        {
            return "Exception: " + Message;
        }
    }
}
