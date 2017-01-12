using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventsApi _eventsApi;
        private readonly IMediator _mediator;

        public EventProcessor(IEventRepository eventRepository, IEventsApi eventsApi, IMediator mediator)
        {
            _eventRepository = eventRepository;
            _eventsApi = eventsApi;
            _mediator = mediator;
        }

        public async Task ProcessEvents()
        {
            var events = await GetEvents();

            if (NoEventsToProcess(events))
            {
                return;
            }

            await CreateRegistrationsFromEvent(events);

            await UpdateLastEventId(events);
        }

        private async Task UpdateLastEventId(IEnumerable<AccountEventView> events)
        {
            var lastEventId = events.Max(x => x.Id);
            await _eventRepository.StoreLastProcessedEventId(lastEventId);
        }

        private async Task CreateRegistrationsFromEvent(IEnumerable<AccountEventView> events)
        {
            foreach (var @event in events)
            {
                await _mediator.SendAsync(new CreateRegistrationCommand {DasAccountId = @event.EmployerAccountId});
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
            var currentEventId = await _eventRepository.GetLastProcessedEventId();
            return currentEventId + 1;
        }
    }
}
