using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Events.Dispatcher;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker
{
    public class EventProcessor : IEventProcessor
    {
        private const string AccountEventsStreamName = "AccountEvents";

        private readonly IEventRepository _eventRepository;
        private readonly IEventsApi _eventsApi;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILog _logger;
        private readonly int _failureTolerance;

        public EventProcessor(IEventRepository eventRepository, IEventsApi eventsApi, IEventDispatcher eventDispatcher, ILog logger, int failureTolerance)
        {
            _eventRepository = eventRepository;
            _eventsApi = eventsApi;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
            _failureTolerance = failureTolerance;
        }

        public async Task ProcessEvents()
        {
            try
            {
                var events = await GetAccountEvents();

                if (NoEventsToProcess(events))
                {
                    _logger.Info("No events to process.");
                    return;
                }

                await HandleEvents(events);

                await UpdateLastProcessedEventId(events);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexcepted exception when processing events.");
            }
        }

        private async Task UpdateLastProcessedEventId(IEnumerable<AccountEventView> events)
        {
            var lastEventId = events.Max(x => x.Id);
            await _eventRepository.StoreLastProcessedEventId(AccountEventsStreamName, lastEventId);
        }

        private async Task HandleEvents(IEnumerable<AccountEventView> events)
        {
            foreach (var @event in events)
            {
                try
                {
                    await HandleEvent(@event);
                    _logger.Info($"Event {@event.Id} processed");
                }
                catch (Exception ex)
                {
                    await HandleEventProcessingException(ex, @event);
                    throw;
                }
            }
        }

        private async Task HandleEvent(AccountEventView @event)
        {
            await _eventDispatcher.Dispatch(@event);
        }

        private async Task HandleEventProcessingException(Exception ex, AccountEventView @event)
        {
            _logger.Error(ex, $"Unexcepted exception when processing event {@event.Id} from event stream {AccountEventsStreamName}.");

            var failureCount = await UpdateFailureCountForEvent(@event.Id);
            if (EventHasExceededFailureTolerance(failureCount))
            {
                _logger.Info($"Event {@event.Id} from event stream {AccountEventsStreamName} has reached the fault tolerance and will no longer be retried.");
                await _eventRepository.StoreLastProcessedEventId(AccountEventsStreamName, @event.Id);
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

        private static bool NoEventsToProcess(IEnumerable<AccountEventView> events)
        {
            return !events.Any();
        }

        private async Task<IEnumerable<AccountEventView>> GetAccountEvents()
        {
            var nextEventId = await GetNextEventId(AccountEventsStreamName);

            var events = await _eventsApi.GetAccountEventsById(nextEventId);
            return events;
        }

        private async Task<long> GetNextEventId(string eventsStream)
        {
            var currentEventId = await _eventRepository.GetLastProcessedEventId(eventsStream);
            return currentEventId + 1;
        }
    }
}
