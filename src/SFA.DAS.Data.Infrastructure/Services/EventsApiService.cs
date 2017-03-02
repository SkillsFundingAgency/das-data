using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using GenericEvent = SFA.DAS.Events.Api.Types.GenericEvent;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class EventsApiService : IEventService
    {
        private const string AprrenticeEventFeedName = "CommitmentApprenticeEvent";
        private readonly IEventsApi _eventsApi;
        private readonly IEventRepository _eventRepository;

        public EventsApiService(IEventsApi eventsApi, IEventRepository eventRepository)
        {
            _eventsApi = eventsApi;
            _eventRepository = eventRepository;
        }

        public async Task SetLastProcessedGenericEventId(string eventType, long id)
        {
            await _eventRepository.StoreLastProcessedEventId($"GenericEvent.{eventType}", id);
        }

        public async Task SetLastProcessedApprenticeshipEventId(long id)
        {
            await _eventRepository.StoreLastProcessedEventId(AprrenticeEventFeedName, id);
        }

        public async Task<ICollection<GenericEvent>> GetUnprocessedGenericEvents(string eventType)
        {
            var eventId = await _eventRepository.GetLastProcessedEventId($"GenericEvent.{eventType}");

            return await _eventsApi.GetGenericEventsById(eventType, eventId);
        }

        public async Task<ICollection<ApprenticeshipEventView>> GetUnprocessedApprenticeshipEvents()
        {
            var eventId = await _eventRepository.GetLastProcessedEventId(AprrenticeEventFeedName);

            return await _eventsApi.GetApprenticeshipEventsById(eventId);
        }
    }
}
