using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.AccountGatewayTests
{
    [TestFixture]
    public class WhenLevyDeclarationsAreRequested : AccountGatewayTestsBase
    {
        [Test]
        public async Task ThenTheLevyDeclarationsAreReturned()
        {
            var expectedLevyDeclarations = new AccountResourceList<LevyDeclarationViewModel>();

            var levyHref = $"/api/accounts/2385/levy/";
                
            AccountApiClient.Setup(x => x.GetResource<AccountResourceList<LevyDeclarationViewModel>>(levyHref)).ReturnsAsync(expectedLevyDeclarations);

            var result = await AccountGateway.GetLevyDeclarations(levyHref);

            result.Should().BeSameAs(expectedLevyDeclarations);
            AccountApiClient.Verify(x => x.GetResource<AccountResourceList<LevyDeclarationViewModel>>(levyHref), Times.Once);
        }
    }
}
