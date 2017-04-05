using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.RenameAccount;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types.Events.Account;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class AccountRenamedEventHandler : EventHandler<GenericEvent<AccountRenamedEvent>>
    {
        private readonly IMediator _mediator;

        public AccountRenamedEventHandler(
            IMediator mediator, 
            IEventRepository eventRepository,
            IDataConfiguration configuration,
            ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(GenericEvent<AccountRenamedEvent> @event)
        {
            await _mediator.PublishAsync(new RenameAccountCommand { AccountHref = @event.Payload.ResourceUri });
        }
    }
}
