using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateEmployerAgreement;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events.Agreement;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class AgreementSignedEventHandler : EventApiHandler<GenericEvent<AgreementSignedEvent>>
    {
        private readonly IMediator _mediator;

        public AgreementSignedEventHandler(
            IMediator mediator,
            IEventRepository eventRepository,
            IDataConfiguration configuration,
            ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(GenericEvent<AgreementSignedEvent> @event)
        {
            await _mediator.PublishAsync(new CreateEmployerAgreementCommand { AgreementHref = @event.Payload.ResourceUrl });
        }
    }
}
