using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class PaymentEventsCollector : IEventsCollector<PeriodEnd>
    {
        private readonly IProviderEventService _eventService;
        private readonly ILog _logger;

        public PaymentEventsCollector(IProviderEventService eventService, ILog logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<ICollection<PeriodEnd>> GetEvents()
        {
            _logger.Info("Getting unprocessed period ends");

            var apiEvents = await _eventService.GetUnprocessedPeriodEnds();

            _logger.Info($"{apiEvents?.Count} periods retrieved from provider events service");

            return apiEvents;
        }
    }
}
