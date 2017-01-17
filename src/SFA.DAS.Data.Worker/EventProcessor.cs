﻿using System;
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

        public EventProcessor(IEventRepository eventRepository, IEventsApi eventsApi, IMediator mediator, ILog logger)
        {
            _eventRepository = eventRepository;
            _eventsApi = eventsApi;
            _mediator = mediator;
            _logger = logger;
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

                await CreateRegistrationsFromEvent(events);

                await UpdateLastEventId(events);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexcepted exception when processing events.");
            }
        }

        private async Task UpdateLastEventId(IEnumerable<AccountEventView> events)
        {
            var lastEventId = events.Max(x => x.Id);
            await _eventRepository.StoreLastProcessedEventId(EventStream, lastEventId);
        }

        private async Task CreateRegistrationsFromEvent(IEnumerable<AccountEventView> events)
        {
            foreach (var @event in events)
            {
                try
                {
                    await _mediator.PublishAsync(new CreateRegistrationCommand {DasAccountId = @event.EmployerAccountId});
                    _logger.Info($"Event {@event.Id} processed");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Unexcepted exception when processing event {@event.Id} from event stream {EventStream}.");
                    await _eventRepository.StoreLastProcessedEventId(EventStream, @event.Id - 1);
                    throw;
                }
            }
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