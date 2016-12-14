using System;
using System.Threading.Tasks;
using SFA.DAS.Messaging;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public static class MessageQueue
    {
        public static PipelineResult<TMessage> WaitFor<TMessage>(
            Func<Task<Message<TMessage>>> recieve) where TMessage : class
        {
            var message = recieve().Result;

            if (message?.Content == null)
                return Result.Fail<TMessage>("Empty message recived");

            return message?.Content.Return(() => { message.AbortAsync(); });
        }
    }
}