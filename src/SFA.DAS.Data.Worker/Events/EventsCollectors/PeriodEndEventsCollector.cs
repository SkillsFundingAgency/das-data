using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class PeriodEndEventsCollector : IEventsCollector<PeriodEnd>
    {
        private readonly IProviderEventService _eventService;
        private readonly ILog _logger;
        private readonly IDataConfiguration _config;

        public PeriodEndEventsCollector(IProviderEventService eventService, ILog logger, IDataConfiguration config)
        {
            _eventService = eventService;
            _logger = logger;
            _config = config;
        }

        public async Task<ICollection<PeriodEnd>> GetEvents()
        {
            _logger.Info("Getting unprocessed period ends");

            if (!_config.PaymentsEnabled && !_config.TransfersEnabled)
            {
                return new List<PeriodEnd>();
            }

            var apiEvents = await _eventService.GetUnprocessedPeriodEnds();

            _logger.Info($"{apiEvents?.Count} periods retrieved from provider events service");

            return apiEvents;
        }
    }
}
