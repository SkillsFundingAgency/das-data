using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.RenameAccount;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.Application.UnitTests.Commands.RenameAccountTests
{
    [TestFixture]
    public class WhenIRenameAnAccount
    {
        private RenameAccountCommandHandler _commandHandler;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<IAccountGateway> _accountGateway;

        [SetUp]
        public void Arrange()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _accountGateway = new Mock<IAccountGateway>();
            
            _commandHandler = new RenameAccountCommandHandler(_accountRepository.Object, _accountGateway.Object);
        }

        [Test]
        public async Task ThenTheAccountDataIsRetrievedAndSaved()
        {
            var expectedAccount = new AccountDetailViewModelBuilder().Build();
            var accountHref = $"/api/accounts/{expectedAccount.HashedAccountId}";
            
            _accountGateway.Setup(x => x.GetAccount(accountHref)).ReturnsAsync(expectedAccount);

            await _commandHandler.Handle(new RenameAccountCommand { AccountHref = accountHref });

            _accountRepository.Verify(x => x.SaveAccount(expectedAccount), Times.Once);
        }
    }
}
