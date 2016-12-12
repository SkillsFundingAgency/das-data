using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Pipeline
{
    //public class Event<T>
    //{
    //    T _instance;

    //    public Event(T instance)
    //    {
    //        _instance = instance;
    //    }

    //    public Event<TO> Bind<TO>(Func<T, Event<TO>> func)
    //    {
    //        return func(_instance);
    //    }

    //    public Event<TO> Map<TO>(Func<T, TO> func)
    //    {
    //        return new Event<TO>(func(_instance));
    //    }
    //}

    //public static class EventExtensions
    //{
    //    public static Event<T> Return<T>(this T instance) => new Event<T>(instance);
    //}

    public abstract class ResultMessage
    {
        public string Message { get; set; }
    }
    public class SuccessResultMessage : ResultMessage { }
    public class FailureResultMessage : ResultMessage { }


    public static class ResultExtensions
    {
        public static Success<T> Return<T>(this T instance)
            => new Success<T>(instance, new List<ResultMessage>());

        //public static Task<Success<T>> Return<T>(this T instance)
        //{
        //    return new Task<Success<T>>(() => new Success<T>(instance, new List<ResultMessage>()));
        //}
    }

    public class Result
    {
        public static Failure<T> FailWith<T>(string message)
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

        protected T _instance;
        public Result<TO> Bind<TO>(Func<T, Result<TO>> func)
        {
            var result = func(_instance);

            _messages.AddRange(result._messages);
            result._messages = _messages.ToList();

            if (result is Success<T>)
            {
                return result;
            }

            return new Failure<TO>(_messages);
        }

        public abstract bool IsSuccess();

        public T Content
        {
            get { return _instance; }
        }
    }

    public class Success<T> : Result<T>
    {
        public Success(T instance, List<ResultMessage> messages)
        {
            _messages = messages;
            _instance = instance;
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
