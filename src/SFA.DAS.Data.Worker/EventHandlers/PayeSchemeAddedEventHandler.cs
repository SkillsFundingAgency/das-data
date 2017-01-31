using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreatePayeScheme;
using SFA.DAS.Data.Worker.Interfaces.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.EventHandlers
{
    public class PayeSchemeAddedEventHandler : IPayeSchemeAddedEventHandler
    {
        private readonly IMediator _mediator;

        public PayeSchemeAddedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(AccountEventView @event)
        {
            await _mediator.PublishAsync(new CreatePayeSchemeCommand { PayeSchemeHref = @event.ResourceUri });
        }
    }
}
