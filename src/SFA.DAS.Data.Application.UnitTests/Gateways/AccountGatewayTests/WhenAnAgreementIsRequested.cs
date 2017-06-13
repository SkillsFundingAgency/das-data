using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.AccountGatewayTests
{
    [TestFixture]
    public class WhenAnAgreementIsRequested : AccountGatewayTestsBase
    {
        [Test]
        public async Task ThenTheAgreementIsReturned()
        {
            var expectedAgreement = new EmployerAgreementView();

            var agreementHref = $"/api/accounts/2385/agreements/123456";
                
            AccountApiClient.Setup(x => x.GetResource<EmployerAgreementView>(agreementHref)).ReturnsAsync(expectedAgreement);

            var result = await AccountGateway.GetEmployerAgreement(agreementHref);

            result.Should().Be(expectedAgreement);
            AccountApiClient.Verify(x => x.GetResource<EmployerAgreementView>(agreementHref), Times.Once);
        }
    }
}
