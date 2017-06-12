using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.AccountGatewayTests
{
    [TestFixture]
    public class WhenAPayeSchemeIsRequested : AccountGatewayTestsBase
    {
        [Test]
        public async Task ThenThePayeSchemeIsReturned()
        {
            var expectedPayeScheme = new PayeSchemeViewModel();

            var payeSchemeHref = $"/api/accounts/2385/payeschemes/123456";
                
            AccountApiClient.Setup(x => x.GetResource<PayeSchemeViewModel>(payeSchemeHref)).ReturnsAsync(expectedPayeScheme);

            var result = await AccountGateway.GetPayeScheme(payeSchemeHref);

            result.Should().Be(expectedPayeScheme);
            AccountApiClient.Verify(x => x.GetResource<PayeSchemeViewModel>(payeSchemeHref), Times.Once);
        }
    }
}
