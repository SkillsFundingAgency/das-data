using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateLevyDeclarations;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events.Levy;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class LevyDeclarationUpdatedEventHandler : EventApiHandler<GenericEvent<LevyDeclarationUpdatedEvent>>
    {
        private readonly IMediator _mediator;

        public LevyDeclarationUpdatedEventHandler(IMediator mediator, IEventRepository eventRepository, IDataConfiguration configuration, ILog logger) : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(GenericEvent<LevyDeclarationUpdatedEvent> @event)
        {
            await _mediator.PublishAsync(new CreateLevyDeclarationsCommand { LevyDeclarationsHref = @event.Payload.ResourceUri });
        }
    }
}
