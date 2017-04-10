using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateTransactions
{
    public class CreateTransactionsCommandHandler : IAsyncNotificationHandler<CreateTransactionsCommand>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountGateway _accountGateway;
        
        public CreateTransactionsCommandHandler(ITransactionRepository transactionRepository, IAccountGateway accountGateway)
        {
            _transactionRepository = transactionRepository;
            _accountGateway = accountGateway;
        }

        public async Task Handle(CreateTransactionsCommand notification)
        {
            var transactions = await _accountGateway.GetTransactions(notification.TransactionsHref);
            await SaveTransactions(transactions);
        }

        private async Task SaveTransactions(List<TransactionViewModel> transactions)
        {
            var tasks = transactions.Select(x => _transactionRepository.SaveTransaction(x));
            await Task.WhenAll(tasks);
        }
    }
}
