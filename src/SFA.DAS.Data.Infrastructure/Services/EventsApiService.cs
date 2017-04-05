using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
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

            return await _eventsApi.GetGenericEventsById(eventType, eventId + 1);
        }

        public async Task<ICollection<ApprenticeshipEventView>> GetUnprocessedApprenticeshipEvents()
        {
            var eventId = await _eventRepository.GetLastProcessedEventId(typeof(ApprenticeshipEventView).Name);

            return await _eventsApi.GetApprenticeshipEventsById(eventId + 1);
        }

        public async Task<ICollection<AccountEventView>> GetUnprocessedAccountEvents()
        {
            var eventId = await _eventRepository.GetLastProcessedEventId(typeof(AccountEventView).Name);

            return await _eventsApi.GetAccountEventsById(eventId + 1);
        }
    }
}
