using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Events.Dispatcher;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events
{
    public class EventsWatcher : IEventsWatcher
    {
        private const string AccountEventsStreamName = "AccountEvents";
        private const string ApprenticeshipEventsStreamName = "ApprenticeshipEvents";

        private readonly IEventRepository _eventRepository;
        private readonly IEventsApi _eventsApi;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILog _logger;
        private readonly int _failureTolerance;

        public EventsWatcher(IEventRepository eventRepository, IEventsApi eventsApi, IEventDispatcher eventDispatcher,
            ILog logger, int failureTolerance)
        {
            _eventRepository = eventRepository;
            _eventsApi = eventsApi;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
            _failureTolerance = failureTolerance;
        }

        //TODO: Rewrite this to support the new event processors structure
        public async Task ProcessEvents()
        {
            try
            {
                //  await ProcessAccountEvents();
                await ProcessApprenticeshipEvents();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexcepted exception when processing events.");
            }
        }

        private async Task ProcessApprenticeshipEvents()
        {
            var apprenticeshipEvents = await GetApprenticeshipEvents();

            if (!apprenticeshipEvents.Any())
            {
                _logger.Info("No events to process.");
                return;
            }

            await HandleEvents(apprenticeshipEvents, ApprenticeshipEventsStreamName);

            await UpdateLastProcessedEventId(apprenticeshipEvents, ApprenticeshipEventsStreamName);
        }

        private async Task ProcessAccountEvents()
        {
            var accountEvents = await GetAccountEvents();

            if (!accountEvents.Any())
            {
                _logger.Info("No events to process.");
                return;
            }

            await HandleEvents(accountEvents, AccountEventsStreamName);

            await UpdateLastProcessedEventId(accountEvents, AccountEventsStreamName);
        }

        private async Task UpdateLastProcessedEventId(IEnumerable<IEventView> events, string eventStreamName)
        {
            var eventViews = events as IEventView[] ?? events.ToArray();

            if (!eventViews.Any())
            {
                return;
            }

            var lastEventId = eventViews.Max(x => x.Id);
            await _eventRepository.StoreLastProcessedEventId(eventStreamName, lastEventId);
        }

        private async Task HandleEvents<T>(IEnumerable<T> events, string eventsStream) where T : IEventView
        {
            foreach (var @event in events)
            {
                try
                {
                    await _eventDispatcher.Dispatch(@event);
                    _logger.Info($"Event {@event.Id} processed");
                }
                catch (Exception ex)
                {
                    await HandleEventProcessingException(ex, @event, eventsStream);
                    throw;
                }
            }
        }

        private async Task HandleEventProcessingException(Exception ex, IEventView @event, string eventsStream)
        {
            _logger.Error(ex,
                $"Unexcepted exception when processing event {@event.Id} from event stream {eventsStream}.");

            var failureCount = await UpdateFailureCountForEvent(@event.Id);
            if (EventHasExceededFailureTolerance(failureCount))
            {
                _logger.Info(
                    $"Event {@event.Id} from event stream {eventsStream} has reached the fault tolerance and will no longer be retried.");
                await _eventRepository.StoreLastProcessedEventId(eventsStream, @event.Id);
            }
            else
            {
                await _eventRepository.StoreLastProcessedEventId(AccountEventsStreamName, @event.Id - 1);
            }
        }

        private bool EventHasExceededFailureTolerance(int failureCount)
        {
            return failureCount >= _failureTolerance;
        }

        private async Task<int> UpdateFailureCountForEvent(long eventId)
        {
            var failureCount = await _eventRepository.GetEventFailureCount(eventId);
            failureCount++;
            await _eventRepository.SetEventFailureCount(eventId, failureCount);
            return failureCount;
        }

        private async Task<ICollection<AccountEventView>> GetAccountEvents()
        {
            var nextEventId = await GetNextEventId(AccountEventsStreamName);

           return await _eventsApi.GetAccountEventsById(nextEventId);
        }

        private async Task<ICollection<ApprenticeshipEventView>> GetApprenticeshipEvents()
        {
            var nextEventId = await GetNextEventId(ApprenticeshipEventsStreamName);
           
            return await _eventsApi.GetApprenticeshipEventsById(nextEventId);
        }

        private async Task<long> GetNextEventId(string eventsStream)
        {
            var currentEventId = await _eventRepository.GetLastProcessedEventId(eventsStream);
            return currentEventId + 1;
        }
    }
}
