using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Events.Dispatcher;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events
{
    public class EventsWatcher : IEventsWatcher
    {
        private readonly IEnumerable<IEventsProcessor> _processors;
        private readonly ILogger _logger;
        //private const string AccountEventsStreamName = "AccountEvents";
        //private const string ApprenticeshipEventsStreamName = "ApprenticeshipEvents";

        //private readonly IEventRepository _eventRepository;
        //private readonly IEventsApi _eventsApi;
        //private readonly IEventDispatcher _eventDispatcher;
        //private readonly ILog _logger;
        //private readonly int _failureTolerance;

        //public EventsWatcher(IEventRepository eventRepository, IEventsApi eventsApi, IEventDispatcher eventDispatcher,
        //    ILog logger, int failureTolerance)
        //{
        //    _eventRepository = eventRepository;
        //    _eventsApi = eventsApi;
        //    _eventDispatcher = eventDispatcher;
        //    _logger = logger;
        //    _failureTolerance = failureTolerance;
        //}

        public EventsWatcher(IEnumerable<IEventsProcessor> processors, ILogger logger)
        {
            _processors = processors;
            _logger = logger;
        }

        public async Task ProcessEvents()
        {
            foreach (var processor in _processors)
            {
                try
                {
                    await processor.ProcessEvents();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred whilst processing events");
                }
            }
        }
       
        //private async Task HandleEventProcessingException(Exception ex, IEventView @event, string eventsStream)
        //{
        //    _logger.Error(ex,
        //        $"Unexcepted exception when processing event {@event.Id} from event stream {eventsStream}.");

        //    var failureCount = await UpdateFailureCountForEvent(@event.Id);
        //    if (EventHasExceededFailureTolerance(failureCount))
        //    {
        //        _logger.Info(
        //            $"Event {@event.Id} from event stream {eventsStream} has reached the fault tolerance and will no longer be retried.");
        //        await _eventRepository.StoreLastProcessedEventId(eventsStream, @event.Id);
        //    }
        //    else
        //    {
        //        await _eventRepository.StoreLastProcessedEventId(AccountEventsStreamName, @event.Id - 1);
        //    }
        //}

        //private bool EventHasExceededFailureTolerance(int failureCount)
        //{
        //    return failureCount >= _failureTolerance;
        //}

        //private async Task<int> UpdateFailureCountForEvent(long eventId)
        //{
        //    var failureCount = await _eventRepository.GetEventFailureCount(eventId);
        //    failureCount++;
        //    await _eventRepository.SetEventFailureCount(eventId, failureCount);
        //    return failureCount;
        //}

       

    }
}
