using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public abstract class EventHandler<T> : IEventHandler<T> where T : IEventView
    {
        protected readonly IEventRepository EventRepository;

        protected EventHandler(IEventRepository eventRepository)
        {
            EventRepository = eventRepository;
        }

        public void Handle(T @event)
        {
            ProcessEvent(@event);
            EventRepository.StoreLastProcessedEventId(nameof(T), @event.Id);
        }

        protected abstract void ProcessEvent(T @event);
    }
}
