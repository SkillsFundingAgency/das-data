using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class ApprenticeshipEventsCollector : IEventsCollector<CommitmentsApprenticeshipEvent>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public ApprenticeshipEventsCollector(IEventService eventService, IMapper mapper, ILog logger)
        {
            _eventService = eventService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<CommitmentsApprenticeshipEvent>> GetEvents()
        {
            _logger.Info("Getting commitment events");

            var apiEvents = await _eventService.GetUnprocessedApprenticeshipEvents();

            _logger.Info($"{apiEvents?.Count} events retrieved from events service");

            var commitmentEvents = apiEvents?.Select(x => _mapper.Map<CommitmentsApprenticeshipEvent>(x))
                                             .ToList();

            return commitmentEvents;
        }
    }
}
