using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateTransactions
{
    public class CreateTransactionsCommand : IAsyncNotification
    {
        public string TransactionsHref { get; set; }
    }
}
