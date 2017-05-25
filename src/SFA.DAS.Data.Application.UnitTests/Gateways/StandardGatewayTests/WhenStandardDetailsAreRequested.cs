using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Data.Application.Gateways;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.StandardGatewayTests
{
    [TestFixture]
    public class WhenStandardDetailsAreRequested
    {
        private StandardGateway _standardGateway;
        private Mock<IStandardApiClient> _standardApiClient;

        [SetUp]
        public void Arrange()
        {
            _standardApiClient = new Mock<IStandardApiClient>();
            _standardGateway = new StandardGateway(_standardApiClient.Object);
        }

        [Test]
        public async Task ThenTheProviderDetailsAreReturned()
        {
            var expectedStandard = new Standard();
            var standardId = "2345743";

            _standardApiClient.Setup(x => x.Get(standardId)).Returns(expectedStandard);

            var result = await _standardGateway.GetStandard(standardId);

            result.Should().Be(expectedStandard);
            _standardApiClient.Verify(x => x.Get(standardId), Times.Once);
        }
    }
}
