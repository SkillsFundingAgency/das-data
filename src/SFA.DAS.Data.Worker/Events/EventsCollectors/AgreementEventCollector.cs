using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class AgreementEventCollector : IEventsCollector<AgreementEventView>
    {
        private readonly IEventService _eventService;
        private readonly ILog _logger;

        public AgreementEventCollector(IEventService eventService, ILog logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<ICollection<AgreementEventView>> GetEvents()
        {
            // This code is commented out to prevent any roatp provider events being picked up
            // as it has not been tested.  To be uncommented when fully tested - Mahinder Suniara
            //_logger.Info("Getting agreement events");

            //var apiEvents = await _eventService.GetUnprocessedAgreementEvents();

            //_logger.Info($"{apiEvents?.Count} Agreement events retrieved from events service");

            //return apiEvents;
            return new List<AgreementEventView>();
        }
    }
}
