﻿using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events
{
    public class EventsProcessor<T> : IEventsProcessor where T : IEventView
    {
        private readonly IEventsCollector<T> _collector;
        private readonly IEventHandler<T> _handler;
        private readonly ILog _logger;

        public EventsProcessor(IEventsCollector<T> collector, IEventHandler<T> handler, ILog logger)
        {
            _collector = collector;
            _handler = handler;
            _logger = logger;
        }

        public async Task ProcessEvents()
        {
            _logger.Info($"Processing {typeof(T).FullName} events");
            
            var events = await _collector.GetEvents();

            if (events != null && events.Any())
            {
                _logger.Info($"Collected {events.Count} events");

                foreach (var @event in events)
                {
                    await _handler.Handle(@event);
                    _logger.Info($"Event {@event.Id} processed");
                }
            }
        }
    }
}
