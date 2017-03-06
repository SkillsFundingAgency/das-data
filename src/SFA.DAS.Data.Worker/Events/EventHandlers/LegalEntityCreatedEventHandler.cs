using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class LegalEntityCreatedEventHandler : EventHandler<LegalEntityCreatedEvent>
    {
        private readonly IMediator _mediator;

        public LegalEntityCreatedEventHandler(
            IMediator mediator, 
            IEventRepository eventRepository,
            IDataConfiguration configuration, 
            ILogger logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(LegalEntityCreatedEvent @event)
        {
            await _mediator.PublishAsync(new CreateLegalEntityCommand { LegalEntityHref = @event.ResourceUri });
        }
    }
}
