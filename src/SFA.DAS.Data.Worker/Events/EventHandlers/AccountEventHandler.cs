using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateAccount;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Application.Commands.CreatePayeScheme;
using SFA.DAS.Data.Application.Commands.RemovePayeScheme;
using SFA.DAS.Data.Application.Commands.RenameAccount;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain.Interfaces.Repositories;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    /// <summary>
    /// This event handler will handle legacy events from the manage apprenticeships project before 
    /// generic events are used. 
    /// TODO: This handler and related event processor should be removed once the support for the legacy events is not needed 
    /// </summary>
    public class AccountEventHandler : EventHandler<AccountEventView>
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;

        public AccountEventHandler(
            IEventRepository eventRepository, 
            IDataConfiguration configuration, 
            IMediator mediator,
            ILog logger) 
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        protected override async Task ProcessEvent(AccountEventView @event)
        {
            switch (@event.Event)
            {
                case "AccountCreated":
                    _logger.Info("AccountCreated account event has been handled");
                    await _mediator.PublishAsync(new CreateAccountCommand { AccountHref = @event.ResourceUri });
                    break;

                case "AccountRenamed":
                    _logger.Info("AccountRenamed account event has been handled");
                    await _mediator.PublishAsync(new RenameAccountCommand { AccountHref = @event.ResourceUri });
                    break;

                case "LegalEntityCreated":
                    _logger.Info("LegalEntityCreated account event has been handled");
                    await _mediator.PublishAsync(new CreateLegalEntityCommand { LegalEntityHref = @event.ResourceUri });
                    break;

                case "PayeSchemeAdded":
                    _logger.Info("PayeSchemeAdded account event has been handled");
                    await _mediator.PublishAsync(new CreatePayeSchemeCommand { PayeSchemeHref = @event.ResourceUri });
                    break;

                case "PayeSchemeRemoved":
                    _logger.Info("PayeSchemeRemoved account event has been handled");
                    await _mediator.PublishAsync(new RemovePayeSchemeCommand { PayeSchemeHref = @event.ResourceUri });
                    break;

                default:
                    _logger.Info($"Unsupported account event has been found of event type {@event.Event}");
                    break;
            }
        }
    }
}
