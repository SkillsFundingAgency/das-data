using SFA.DAS.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Pipeline
{
    public class Pipeline<TMessage, TResource, TStaging> 
        where TStaging : class 
        where TMessage : class
        where TResource : class
    {
        public Pipeline(
            string pipelineName, 
            Action<Pipeline<TMessage, TResource, TStaging>> configure)
        {
            PipelineName = pipelineName;
            Transform = r => new Task<TStaging>(() => r as TStaging);
            Log = (l,m) => { };
            configure(this);
        }
        
        public string PipelineName { get; set; }

        public Func<Task<Message<TMessage>>> Receive { get; private set; }

        public Func<TMessage, Task<TResource>> GetResource { get; private set; }

        public Func<TResource, Task<TStaging>> Transform { get; private set; }

        public Func<TStaging, Task> Store { get; private set; }

        public Action<LogLevel,string> Log { get; set; }

        public void Init()
        {
            //check that the pipeline is configured in a sane way
        }


        public async Task Handle()
        {
            Message<TMessage> message = null;

            try
            {
                message = await Receive();
                if (message?.Content == null) return;
                Log(LogLevel.Info, PipelineName + " recieved message");

                var resource = await GetResource(message.Content);
                Log(LogLevel.Info, PipelineName + " fetched resource");

                var staging = await Transform(resource);
                Log(LogLevel.Info, PipelineName + " transformed resource");

                await Store(staging);
                Log(LogLevel.Info, PipelineName + " stored data");
            }
            catch (Exception e)
            {
                Log(LogLevel.Error, e.Message);
                if (message != null) await message.AbortAsync();
                throw;
            }
        }
    }
}