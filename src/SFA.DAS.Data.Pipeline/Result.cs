using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    public static class ResultExtensions
    {
        public static Success<T> Return<T>(this T instance)
            => new Success<T>(instance, new List<ResultMessage>());
    }

    public class Result
    {
        public static Failure<T> Fail<T>(string message)
        {
            return new Failure<T>(new List<ResultMessage> { new FailureResultMessage { Message = message } });
        }

        public static Success<T> Win<T>(T result, string message)
        {
            return new Success<T>(result, new List<ResultMessage> { new SuccessResultMessage { Message = message } });
        }
    }

    public abstract class Result<T>
    {
        protected List<ResultMessage> _messages = new List<ResultMessage>();

        protected T Instance;
        public Result<TO> Bind<TO>(Func<T, Result<TO>> func)
        {
            var result = func(Instance);

            _messages.AddRange(result._messages);
            result._messages = _messages.ToList();

            if (result is Success<T>)
                return result;

            return new Failure<TO>(_messages);
        }

        public abstract bool IsSuccess();

        public T Content => Instance;

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
            Instance = instance;
        }

        public override bool IsSuccess() => true;
    }

    public class Failure<T> : Result<T>
    {
        public Failure(List<ResultMessage> messages)
        {
            _messages = messages;
        }

        public override bool IsSuccess() => false;
    }
}
