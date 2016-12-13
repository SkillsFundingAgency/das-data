using System;
using System.Threading.Tasks;
using SFA.DAS.Messaging;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    // build a message helper that allows a return from 
    // a Func<Task<Message<TMessage>>>

    //need a rollback result

    public static class MessageQueue
    {
        public static PipelineResult<TMessage> WaitFor<TMessage>(Func<Task<Message<TMessage>>> recieve) where TMessage : class
        {
            var message = recieve().Result;

            if (message?.Content == null)
                return Result.Fail<TMessage>("Empty message recived");

            return message?.Content.Return(() => { message.AbortAsync(); });
        }
    }
}