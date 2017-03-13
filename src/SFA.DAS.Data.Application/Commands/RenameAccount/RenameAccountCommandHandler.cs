using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Domain.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.RenameAccount
{
    public class RenameAccountCommandHandler : IAsyncNotificationHandler<RenameAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGateway _accountGateway;
        
        public RenameAccountCommandHandler(IAccountRepository accountRepository, IAccountGateway accountGateway)
        {
            _accountRepository = accountRepository;
            _accountGateway = accountGateway;
        }

        public async Task Handle(RenameAccountCommand notification)
        {
            var account = await _accountGateway.GetAccount(notification.AccountHref);
            await _accountRepository.SaveAccount(account);
        }
    }
}
