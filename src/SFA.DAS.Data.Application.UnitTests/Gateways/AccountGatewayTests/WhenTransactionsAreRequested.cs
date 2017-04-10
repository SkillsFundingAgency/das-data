using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.AccountGatewayTests
{
    [TestFixture]
    public class WhenTransactionsAreRequested : AccountGatewayTestsBase
    {
        [Test]
        public async Task ThenTheTransactionsAreReturned()
        {
            var expectedTransactions = new AccountResourceList<TransactionViewModel>();

            var transactionsHref = $"/api/accounts/2385/transactions/";
                
            AccountApiClient.Setup(x => x.GetResource<AccountResourceList<TransactionViewModel>>(transactionsHref)).ReturnsAsync(expectedTransactions);

            var result = await AccountGateway.GetTransactions(transactionsHref);

            result.Should().BeSameAs(expectedTransactions);
            AccountApiClient.Verify(x => x.GetResource<AccountResourceList<TransactionViewModel>>(transactionsHref), Times.Once);
        }
    }
}
