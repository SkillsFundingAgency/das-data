using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Data.Pipeline
{
    public abstract class ResultMessage
    {
        public string Message { get; set; }
    }
    public class SuccessResultMessage : ResultMessage
    {
        public override string ToString() => "Success: " + Message;
    }
    public class FailureResultMessage : ResultMessage
    {
        public override string ToString() => "Failure: " + Message;
    }

    public class ExceptionResultMessage : ResultMessage
    {
        public override string ToString() => "Exception: " + Message;
    }


    public static class ResultExtensions
    {
        public static Success<T> Return<T>(this T instance)
            => new Success<T>(instance, new List<ResultMessage>());

        public static Success<T> Return<T>(this T instance, Action rolback)
            => new Success<T>(instance, new List<ResultMessage>(), rolback);
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
        protected List<ResultMessage> _messages = new List<ResultMessage>();
        protected List<Action> _rollbacks = new List<Action>();

        protected T _instance;
        public PipelineResult<TO> Step<TO>(Func<T, PipelineResult<TO>> func)
        {
            if (!IsSuccess())
                return new Failure<TO>(_messages);

            try
            {
                var result = func(_instance);

                _messages.AddRange(result._messages);
                result._messages = _messages.ToList();

                _rollbacks.AddRange(result._rollbacks);
                result._rollbacks = _rollbacks.ToList();

                if (result is Success<TO>)
                    return result;

                foreach (var rollback in _rollbacks)
                    rollback();

                return new Failure<TO>(_messages);
            }
            catch (Exception ex)
            {
                return new Failure<TO>(ex);
            }
        }

        public PipelineResult<TO> Step<TO>(Func<T, PipelineResult<TO>> func, Action rollback)
        {
            _rollbacks.Add(rollback);
            return Step(func);
        }

        public abstract bool IsSuccess();

        public virtual T Content => _instance;

        public IEnumerable<string> Messages
        {
            get { return _messages.Select(m => m.ToString()); }
        }
    }

    public class Success<T> : PipelineResult<T>
    {
        public Success(T instance, List<ResultMessage> messages)
        {
            _messages = messages;
            _instance = instance;
        }

        public Success(T instance, List<ResultMessage> messages, Action rollback) : this(instance, messages)
        {
            _rollbacks.Add(rollback);
        } 

        public Success(T instance, string message) : this (instance, 
            new List<ResultMessage> { new SuccessResultMessage { Message = message } }){}

        public override bool IsSuccess() => true;
    }

    public class Failure<T> : PipelineResult<T>
    {
        public Failure(List<ResultMessage> messages)
        {
            _messages = messages;
        }

        public Failure(string message) : this(new List<ResultMessage>
            { new FailureResultMessage {Message = message} }) {}

        public Failure(Exception e) : this(new List<ResultMessage>
            { new ExceptionResultMessage {Message = e.Message} }) {}

        public override bool IsSuccess() => false;
    }
}
