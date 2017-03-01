using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Worker.Interfaces.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class LegalEntityCreatedEventHandler : ILegalEntityCreatedEventHandler
    {
        private readonly IMediator _mediator;

        public LegalEntityCreatedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(AccountEventView @event)
        {
            await _mediator.PublishAsync(new CreateLegalEntityCommand { LegalEntityHref = @event.ResourceUri });
        }
    }
}
