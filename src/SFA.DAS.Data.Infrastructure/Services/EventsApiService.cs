using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using GenericEvent = SFA.DAS.Events.Api.Types.GenericEvent;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class EventsApiService : IEventService
    {
        private readonly IEventsApi _eventsApi;

        public EventsApiService(IEventsApi eventsApi)
        {
            _eventsApi = eventsApi;
        }

        //TODO: finish this off
        public Task<ICollection<GenericEvent>> GetGenericEvents(string eventType)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ApprenticeshipEventView>> GetApprenticeshipEvents()
        {
            throw new NotImplementedException();
        }
    }
}
