using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Worker.EventsCollectors
{
    public class ApprenticeshipEventsCollector : IEventsCollector<CommitmentsApprenticeshipEvent>
    {
        private readonly IEventService _eventService;

        public ApprenticeshipEventsCollector(IEventService eventService)
        {
            _eventService = eventService;
        }

        public Task<ICollection<CommitmentsApprenticeshipEvent>> GetEvents()
        {
            //return _eventService.GetApprenticeshipEvents();
            throw new NotImplementedException();
        }
    }
}
