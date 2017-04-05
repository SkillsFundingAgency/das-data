using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public abstract class EventHandler<TEventType>
    {
        protected readonly IEventRepository EventRepository;
        private readonly int _failureRetryLimit;
        private readonly ILog _logger;

        protected EventHandler(IEventRepository eventRepository, IDataConfiguration configuration, ILog logger)
        {
            EventRepository = eventRepository;
            _failureRetryLimit = configuration?.FailureTolerance ?? 1;
            _logger = logger;
        }

        protected async Task Handle<TIdType>(TEventType @event, string eventType, TIdType eventId)
        {
            try
            {
                await ProcessEvent(@event);
                await EventRepository.StoreLastProcessedEventId(eventType, eventId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error handling event of type {eventType} with id {eventId}");

                await CheckProcessEventRetryAllowed(eventType, eventId);

                throw;
            }
        }

        protected abstract Task ProcessEvent(TEventType @event);

        private async Task CheckProcessEventRetryAllowed<TIdType>(string eventType, TIdType eventId)
        {
            if (await IncreamentFailureCountForEvent(eventId) > _failureRetryLimit)
            {
                //Too many failures so ignore event
                await EventRepository.StoreLastProcessedEventId(eventType, eventId);
                await EventRepository.SetEventFailureCount(eventId, 0);
            }
        }

        private async Task<int> IncreamentFailureCountForEvent<TIdType>(TIdType id)
        {
            var failureCount = await EventRepository.GetEventFailureCount(id);
            await EventRepository.SetEventFailureCount(id, ++failureCount);

            return failureCount;
        }
    }
}
