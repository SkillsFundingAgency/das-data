using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.RenameAccount;
using SFA.DAS.Data.Worker.Interfaces.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.EventHandlers
{
    public class AccountRenamedEventHandler : IAccountRenamedEventHandler
    {
        private readonly IMediator _mediator;

        public AccountRenamedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(AccountEventView @event)
        {
            await _mediator.PublishAsync(new RenameAccountCommand { AccountHref = @event.ResourceUri });
        }
    }
}
