using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class ApprenticeshipEventsCollector : IEventsCollector<ApprenticeshipEventView>
    {
        private readonly IEventService _eventService;
        private readonly ILog _logger;

        public ApprenticeshipEventsCollector(IEventService eventService, ILog logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<ICollection<ApprenticeshipEventView>> GetEvents()
        {
            _logger.Info("Getting commitment events");

            var apiEvents = await _eventService.GetUnprocessedApprenticeshipEvents();

            _logger.Info($"{apiEvents?.Count} events retrieved from events service");

            return apiEvents;
        }
    }
}
