using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateAccount;
using SFA.DAS.Data.Worker.Interfaces.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class AccountCreatedEventHandler : IAccountCreatedEventHandler
    {
        private readonly IMediator _mediator;

        public AccountCreatedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(AccountEventView @event)
        {
            await _mediator.PublishAsync(new CreateAccountCommand { AccountHref = @event.ResourceUri });
        }
    }
}
