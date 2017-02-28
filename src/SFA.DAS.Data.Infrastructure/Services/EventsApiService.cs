using System;
using System.Collections.Generic;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Events.Api.Client;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class EventsApiService : IEventService
    {
        private readonly IEventsApi _eventsApi;

        public EventsApiService(IEventsApi eventsApi)
        {
            _eventsApi = eventsApi;
        }

        public ICollection<T> GetEvents<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<CommitmentsApprenticeshipEvent> GetApprenticeshipEvents()
        {
            throw new NotImplementedException();
        }
    }
}
