using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class EventsApiService : IEventService
    {
        private readonly IEventsApi _eventsApi;
        private readonly IEventRepository _eventRepository;

        public EventsApiService(IEventsApi eventsApi, IEventRepository eventRepository)
        {
            _eventsApi = eventsApi;
            _eventRepository = eventRepository;
        }

     

        public async Task<ICollection<GenericEvent>> GetUnprocessedGenericEvents(string eventType)
        {
            var eventId = await _eventRepository.GetLastProcessedEventId(eventType);

            return await _eventsApi.GetGenericEventsById(eventType, eventId);
        }

        public async Task<ICollection<ApprenticeshipEventView>> GetUnprocessedApprenticeshipEvents()
        {
            var eventId = await _eventRepository.GetLastProcessedEventId(nameof(CommitmentsApprenticeshipEvent));

            return await _eventsApi.GetApprenticeshipEventsById(eventId);
        }
    }
}
