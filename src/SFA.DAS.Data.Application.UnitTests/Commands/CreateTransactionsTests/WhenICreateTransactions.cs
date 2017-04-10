using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateTransactions;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateTransactionsTests
{
    [TestFixture]
    public class WhenICreateTransactions
    {
        private CreateTransactionsCommandHandler _commandHandler;
        private Mock<ITransactionRepository> _transactionRepository;
        private Mock<IAccountGateway> _accountGateway;
        
        [SetUp]
        public void Arrange()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _accountGateway = new Mock<IAccountGateway>();
            
            _commandHandler = new CreateTransactionsCommandHandler(_transactionRepository.Object, _accountGateway.Object);
        }

        [Test]
        public async Task ThenTheTransactionsAreRetrievedAndSaved()
        {
            var transactions = new List<TransactionViewModel>
            {
                new TransactionViewModel(),
                new TransactionViewModel(),
                new TransactionViewModel()
            };
            var transactionsHref = $"/api/accounts/ABC123/transactions/";
            
            _accountGateway.Setup(x => x.GetTransactions(transactionsHref)).ReturnsAsync(transactions);

            await _commandHandler.Handle(new CreateTransactionsCommand { TransactionsHref = transactionsHref });

            _transactionRepository.Verify(x => x.SaveTransaction(It.IsAny<TransactionViewModel>()), Times.Exactly(3));
            _transactionRepository.Verify(x => x.SaveTransaction(transactions[0]), Times.Once);
            _transactionRepository.Verify(x => x.SaveTransaction(transactions[1]), Times.Once);
            _transactionRepository.Verify(x => x.SaveTransaction(transactions[2]), Times.Once);
        }
    }
}
