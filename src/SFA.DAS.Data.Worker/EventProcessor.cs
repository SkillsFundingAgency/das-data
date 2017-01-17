using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker
{
    public class EventProcessor : IEventProcessor
    {
        private const string EventStream = "AccountEvents";

        private readonly IEventRepository _eventRepository;
        private readonly IEventsApi _eventsApi;
        private readonly IMediator _mediator;
        private readonly ILog _logger;
        private readonly int _failureTolerance;

        public EventProcessor(IEventRepository eventRepository, IEventsApi eventsApi, IMediator mediator, ILog logger, int failureTolerance)
        {
            _eventRepository = eventRepository;
            _eventsApi = eventsApi;
            _mediator = mediator;
            _logger = logger;
            _failureTolerance = failureTolerance;
        }

        public async Task ProcessEvents()
        {
            try
            {
                var events = await GetEvents();

                if (NoEventsToProcess(events))
                {
                    _logger.Info("No events to process.");
                    return;
                }

                await CreateRegistrationsFromEvents(events);

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
            await _eventRepository.StoreLastProcessedEventId(EventStream, lastEventId);
        }

        private async Task CreateRegistrationsFromEvents(IEnumerable<AccountEventView> events)
        {
            foreach (var @event in events)
            {
                try
                {
                    await CreateRegistration(@event);
                    _logger.Info($"Event {@event.Id} processed");
                }
                catch (Exception ex)
                {
                    await HandleEventProcessingException(ex, @event);
                    throw;
                }
            }
        }

        private async Task CreateRegistration(AccountEventView @event)
        {
            await _mediator.PublishAsync(new CreateRegistrationCommand {DasAccountId = @event.EmployerAccountId});
        }

        private async Task HandleEventProcessingException(Exception ex, AccountEventView @event)
        {
            _logger.Error(ex, $"Unexcepted exception when processing event {@event.Id} from event stream {EventStream}.");

            var failureCount = await UpdateFailureCountForEvent(@event.Id);
            if (EventHasExceededFailureTolerance(failureCount))
            {
                _logger.Info($"Event {@event.Id} from event stream {EventStream} has reached the fault tolerance and will no longer be retried.");
                await _eventRepository.StoreLastProcessedEventId(EventStream, @event.Id);
            }
            else
            {
                await _eventRepository.StoreLastProcessedEventId(EventStream, @event.Id - 1);
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

        private async Task<IEnumerable<AccountEventView>> GetEvents()
        {
            var nextEventId = await GetNextEventId();

            var events = await _eventsApi.GetAccountEventsById(nextEventId);
            return events;
        }

        private async Task<long> GetNextEventId()
        {
            var currentEventId = await _eventRepository.GetLastProcessedEventId(EventStream);
            return currentEventId + 1;
        }
    }
}
