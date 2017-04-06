using System.Threading.Tasks;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public abstract class EventApiHandler<T> : EventHandler<T> where T : IEventView
    {
        protected EventApiHandler(IEventRepository eventRepository, IDataConfiguration configuration, ILog logger) : base(eventRepository, configuration, logger)
        {
        }

        public override async Task Handle(T @event)
        {
            await Handle(@event, @event.Type, @event.Id);
        }
    }
}
