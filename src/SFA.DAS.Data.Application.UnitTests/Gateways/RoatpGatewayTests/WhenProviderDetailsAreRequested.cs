using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;
using SFA.Roatp.Api.Client;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    [TestFixture]
    public class WhenProviderDetailsAreRequested
    {
        private RoatpGateway _roatpGateway;
        private Mock<IRoatpClient> _roatpApiClient;

        [SetUp]
        public void Arrange()
        {
            _roatpApiClient = new Mock<IRoatpClient>();
            _roatpGateway = new RoatpGateway(_roatpApiClient.Object);
        }

        [Test]
        public async Task ThenTheProviderDetailsAreReturned()
        {
            var expectedProvider = new Roatp.Api.Types.Provider();
            long providerId = 2345743;

            _roatpApiClient.Setup(x => x.Get(providerId)).Returns(expectedProvider);

            var result = await _roatpGateway.GetProvider(providerId);

            result.Should().Be(expectedProvider);
            _roatpApiClient.Verify(x => x.Get(providerId), Times.Once);
        }
    }
}
