using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.AccountGatewayTests
{
    [TestFixture]
    public class WhenALegalEntityIsRequested : AccountGatewayTestsBase
    {
        [Test]
        public async Task ThenTheLegalEntityIsReturned()
        {
            var expectedLegalEntity = new LegalEntityViewModel();

            var legalEntityHref = $"/api/accounts/2385/legalentities/123456";
                
            AccountApiClient.Setup(x => x.GetResource<LegalEntityViewModel>(legalEntityHref)).ReturnsAsync(expectedLegalEntity);

            var result = await AccountGateway.GetLegalEntity(legalEntityHref);

            result.Should().Be(expectedLegalEntity);
            AccountApiClient.Verify(x => x.GetResource<LegalEntityViewModel>(legalEntityHref), Times.Once);
        }
    }
}
