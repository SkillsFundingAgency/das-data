using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.AccountGatewayTests
{
    [TestFixture]
    public class WhenAccountDetailsAreRequested : AccountGatewayTestsBase
    {
        [Test]
        public async Task ThenTheAccountDetailsAreReturned()
        {
            var expectedAccountInformation = new AccountDetailViewModelBuilder().Build();

            var accountHref = $"/api/accounts/2385";
                
            AccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(accountHref)).ReturnsAsync(expectedAccountInformation);

            var result = await AccountGateway.GetAccount(accountHref);

            result.Should().Be(expectedAccountInformation);
            AccountApiClient.Verify(x => x.GetResource<AccountDetailViewModel>(accountHref), Times.Once);
        }
    }
}
