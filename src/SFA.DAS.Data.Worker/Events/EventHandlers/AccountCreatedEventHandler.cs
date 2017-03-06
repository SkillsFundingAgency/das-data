using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.Data.Application.Commands.CreateAccount;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class AccountCreatedEventHandler : EventHandler<AccountCreatedEvent>
    {
        private readonly IMediator _mediator;

        public AccountCreatedEventHandler(
            IMediator mediator,
            IEventRepository eventRepository,
            IDataConfiguration configuration, 
            ILogger logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(AccountCreatedEvent @event)
        {
            await _mediator.PublishAsync(new CreateAccountCommand { AccountHref = @event.ResourceUri });
        }
    }
}
