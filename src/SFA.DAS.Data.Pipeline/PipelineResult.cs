using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Data.Pipeline
{
    public static class ResultExtensions
    {
        public static Success<T> Return<T>(
            this T instance, Action<LogLevel,string> log = null)
            => new Success<T>(instance, "",log);

        public static Success<T> Return<T>(
            this T instance, Action rolback, Action<LogLevel, string> log = null)
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
        protected string _message;
        protected List<Action> _rollbacks = new List<Action>();
        protected Action<LogLevel, string> _log;

        protected T _instance;
        public PipelineResult<TO> Step<TO>(Func<T, PipelineResult<TO>> func)
        {
            if (!IsSuccess())
                return new Failure<TO>(_message,_log);
            try
            {
                var result = func(_instance);
                
                _rollbacks.AddRange(result._rollbacks);
                result._rollbacks = _rollbacks.ToList();
                result._log = _log;

                if (result is Success<TO>)
                {
                    _log(LogLevel.Info, result.ToString());
                    return result;
                }

                _log(LogLevel.Error, result.ToString());
                foreach (var rollback in _rollbacks)
                    rollback();

                return new Failure<TO>(_message, _log);
            }
            catch (Exception ex)
            {
                var result = new ExceptionFailure<TO>(ex,_log);
                _log(LogLevel.Error, result.ToString());
                return result;
            }
        }

        public PipelineResult<TO> Step<TO>(Func<T, PipelineResult<TO>> func, Action rollback)
        {
            _rollbacks.Add(rollback);
            return Step(func);
        }

        public abstract bool IsSuccess();

        public virtual T Content => _instance;
    }

    public class Success<T> : PipelineResult<T>
    {
        public Success(T instance, string message, Action<LogLevel, string> log = null)
        {
            _log = log ?? ((level, s) => { });
            _message = message;
            _instance = instance;
        }

        public Success(
            T instance, string message, Action rollback, Action<LogLevel,string> log = null) : this(instance, message)
        {
            _log = log ?? ((level, s) => { });
            _rollbacks.Add(rollback);
        }

        public override string ToString()
        {
            return "Success: " + _message;
        }

        public override bool IsSuccess() => true;
    }

    public class Failure<T> : PipelineResult<T>
    {
        public Failure(string message, Action<LogLevel, string> log = null)
        {
            _log = log ?? ((level, s) => { });
            _message = message;
        }
        
        public override string ToString()
        {
            return "Failure: " + _message;
        }

        public override bool IsSuccess() => false;
    }

    public class ExceptionFailure<T> : Failure<T>
    {
        public ExceptionFailure(string message, Action<LogLevel, string> log = null) : base(message)
        {
            _log = log ?? ((level, s) => { });
        }

        public ExceptionFailure(Exception e, Action<LogLevel, string> log = null) : this(e.Message, log)
        {
            _log = log ?? ((level, s) => { });
        }

        public override string ToString()
        {
            return "Exception: " + _message;
        }
    }
}
