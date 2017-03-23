using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.Data.Application.Commands.CreateAccount;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class AccountCreatedEventHandler : EventHandler<GenericEvent<AccountCreatedEvent>>
    {
        private readonly IMediator _mediator;

        public AccountCreatedEventHandler(
            IMediator mediator,
            IEventRepository eventRepository,
            IDataConfiguration configuration,
            ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(GenericEvent<AccountCreatedEvent> @event)
        {
            await _mediator.PublishAsync(new CreateAccountCommand { AccountHref = @event.Payload.ResourceUri });
        }
    }
}
