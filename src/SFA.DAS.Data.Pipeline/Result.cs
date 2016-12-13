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
    }

    public class Result
    {
        public static Failure<string> Fail(string message)
        {
            return new Failure<string>(message);
        }

        public static Success<T> Win<T>(T result, string message)
        {
            return new Success<T>(result, message);
        }
    }

    public abstract class Result<T>
    {
        protected List<ResultMessage> _messages = new List<ResultMessage>();

        protected T _instance;
        public Result<TO> Bind<TO>(Func<T, Result<TO>> func)
        {
            try
            {
                var result = func(_instance);

                _messages.AddRange(result._messages);
                result._messages = _messages.ToList();

                if (result is Success<T>)
                    return result;

                return new Failure<TO>(_messages);
            }
            catch (Exception ex)
            {
                return new Failure<TO>(ex);
            }
        }

        public abstract bool IsSuccess();

        public virtual T Content => _instance;

        public IEnumerable<string> Messages
        {
            get { return this._messages.Select(m => m.ToString()); }
        }
    }

    public class Success<T> : Result<T>
    {
        public Success(T instance, List<ResultMessage> messages)
        {
            _messages = messages;
            _instance = instance;
        }

        public Success(T instance, string message) : this (instance, 
            new List<ResultMessage> { new SuccessResultMessage { Message = message } }){}
        
        public override bool IsSuccess() => true;
    }

    public class Failure<T> : Result<T>
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
