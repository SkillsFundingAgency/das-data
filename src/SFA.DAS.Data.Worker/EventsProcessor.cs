using System.Threading.Tasks;
using SFA.DAS.Data.Worker.EventHandlers;
using SFA.DAS.Data.Worker.EventsCollectors;

namespace SFA.DAS.Data.Worker
{
    public class EventsProcessor<T> : IEventsProcessor
    {
        private readonly IEventsCollector<T> _collector;
        private readonly IEventHandler<T> _handler;

        public EventsProcessor(IEventsCollector<T> collector, IEventHandler<T> handler)
        {
            _collector = collector;
            _handler = handler;
        }

        public async Task ProcessEvents()
        {
            var events = await _collector.GetEvents();

            foreach (var @event in events)
            {
                _handler.Handle(@event);
            }
        }
    }
}
