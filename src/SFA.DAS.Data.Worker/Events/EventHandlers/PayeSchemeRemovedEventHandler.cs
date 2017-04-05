using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.RemovePayeScheme;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events.PayeScheme;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class PayeSchemeRemovedEventHandler : EventHandler<GenericEvent<PayeSchemeRemovedEvent>>
    {
        private readonly IMediator _mediator;

        public PayeSchemeRemovedEventHandler(
            IMediator mediator,
            IEventRepository eventRepository,
            IDataConfiguration configuration,
            ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(GenericEvent<PayeSchemeRemovedEvent> @event)
        {
            await _mediator.PublishAsync(new RemovePayeSchemeCommand { PayeSchemeHref = @event.Payload.ResourceUri });
        }
    }
}
