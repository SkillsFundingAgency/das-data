using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Worker.Factories;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class GenericEventCollector<T> : IEventsCollector<T>
    {
        private readonly IEventService _eventService;
        private readonly IEventModelFactory _factory;
        private readonly ILogger _logger;

        public GenericEventCollector(IEventService eventService, IEventModelFactory factory, ILogger logger)
        {
            _eventService = eventService;
            _factory = factory;
            _logger = logger;
        }

        public async Task<ICollection<T>> GetEvents()
        {
            _logger.Info($"Getting events from events service of type {typeof(T).Name}");

            var events = await _eventService.GetGenericEvents(typeof(T).Name);

            var eventModels = events.Select(x => _factory.Create<T>(x.Payload))
                                    .ToList();
            return eventModels;
        }
    }
}
