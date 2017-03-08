using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class AccountEventCollector : IEventsCollector<AccountEventView>
    {
        private readonly IEventService _eventService;
        private readonly ILog _logger;

        public AccountEventCollector(IEventService eventService, ILog logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<ICollection<AccountEventView>> GetEvents()
        {
            _logger.Info("Getting account events");

            var apiEvents = await _eventService.GetUnprocessedAccountEvents();

            _logger.Info($"{apiEvents?.Count} events retrieved from events service");

            return apiEvents;
        }
    }
}
