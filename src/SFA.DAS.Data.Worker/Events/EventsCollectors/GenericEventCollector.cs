using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Factories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class GenericEventCollector<T> : IEventsCollector<GenericEvent<T>>
    {
        private readonly IEventService _eventService;
        private readonly IEventModelFactory _factory;
        private readonly ILog _logger;

        public GenericEventCollector(IEventService eventService, IEventModelFactory factory, ILog logger)
        {
            _eventService = eventService;
            _factory = factory;
            _logger = logger;
        }

        public async Task<ICollection<GenericEvent<T>>> GetEvents()
        {
            var typeName = typeof(T).Name;

            _logger.Info($"Getting events from events service of type {typeName}");

            var events = await _eventService.GetUnprocessedGenericEvents(typeName);

            var eventModels = events?.Select(x => new GenericEvent<T> { Id = x.Id, Payload = _factory.Create<T>(x.Payload), Type = x.Type }).ToList();
            return eventModels;
        }
    }
}
