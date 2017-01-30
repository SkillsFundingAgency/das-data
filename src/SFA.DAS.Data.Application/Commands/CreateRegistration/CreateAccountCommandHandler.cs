using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Application.Commands.CreatePayeScheme;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateAccountCommandHandler : IAsyncNotificationHandler<CreateAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGateway _accountGateway;
        private readonly IMediator _mediator;

        public CreateAccountCommandHandler(IAccountRepository accountRepository, IAccountGateway accountGateway, IMediator mediator)
        {
            _accountRepository = accountRepository;
            _accountGateway = accountGateway;
            _mediator = mediator;
        }

        public async Task Handle(CreateAccountCommand notification)
        {
            var account = await _accountGateway.GetAccount(notification.AccountHref);
            await _accountRepository.SaveAccount(account);
            await SaveLegalEntities(account.LegalEntities);
            await SavePayeSchemes(account.PayeSchemes);
        }

        private async Task SavePayeSchemes(ResourceList accountPayeSchemes)
        {
            foreach (var payeScheme in accountPayeSchemes)
            {
                await _mediator.PublishAsync(new CreatePayeSchemeCommand { PayeSchemeHref = payeScheme.Href });
            }
        }

        private async Task SaveLegalEntities(ResourceList accountLegalEntities)
        {
            foreach (var legalEntity in accountLegalEntities)
            {
                await _mediator.PublishAsync(new CreateLegalEntityCommand { LegalEntityHref = legalEntity.Href });
            }
        }
    }
}
