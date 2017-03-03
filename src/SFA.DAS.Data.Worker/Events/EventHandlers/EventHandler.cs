using System;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public abstract class EventHandler<T> : IEventHandler<T> where T : IEventView
    {
        protected readonly IEventRepository EventRepository;
        private readonly int _failureRetryLimit;
        private readonly ILogger _logger;

        protected EventHandler(IEventRepository eventRepository, IDataConfiguration configuration, ILogger logger)
        {
            EventRepository = eventRepository;
            _failureRetryLimit = configuration?.FailureTolerance ?? 1;
            _logger = logger;
        }

        public async Task Handle(T @event)
        {
            try
            {
                await ProcessEvent(@event);
                await EventRepository.StoreLastProcessedEventId(nameof(T), @event.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error handling event of type {nameof(T)} with id {@event.Id}");

                await CheckProcessEventRetryAllowed(@event);

                throw;
            }
        }

        protected abstract Task ProcessEvent(T @event);

        private async Task CheckProcessEventRetryAllowed(T @event)
        {
            if (await IncreamentFailureCountForEvent(@event.Id) > _failureRetryLimit)
            {
                var eventType = typeof(T).Name;
                
                //Too many failures so ignore event
                await EventRepository.StoreLastProcessedEventId(eventType, @event.Id);
                await EventRepository.SetEventFailureCount(@event.Id, 0);
            }
        }

        private async Task<int> IncreamentFailureCountForEvent(long id)
        {
            var failureCount = await EventRepository.GetEventFailureCount(id);
            await EventRepository.SetEventFailureCount(id, ++failureCount);

            return failureCount;
        }
    }
}
