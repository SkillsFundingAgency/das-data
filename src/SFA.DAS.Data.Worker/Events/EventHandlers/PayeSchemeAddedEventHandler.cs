using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.Data.Application.Commands.CreatePayeScheme;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class PayeSchemeAddedEventHandler : EventHandler<PayeSchemeAddedEvent>
    {
        private readonly IMediator _mediator;

        public PayeSchemeAddedEventHandler(
            IMediator mediator,
            IEventRepository eventRepository,
            IDataConfiguration configuration,
            ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(PayeSchemeAddedEvent @event)
        {
            await _mediator.PublishAsync(new CreatePayeSchemeCommand { PayeSchemeHref = @event.ResourceUri });
        }
    }
}
