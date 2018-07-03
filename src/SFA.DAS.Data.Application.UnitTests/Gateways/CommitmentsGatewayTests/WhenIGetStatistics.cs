using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.CommitmentsGatewayTests
{
    [TestFixture]
    public class WhenIGetStatistics : CommitmentsGatewayTestBase
    {
        [Test]
        public void ShouldReturnStatistics()
        {
            var statistics = CommitmentsGateway.GetStatistics();
            Assert.IsNotNull(statistics);
            Assert.AreSame(statistics.Result, ExpectedStatistics);
        }
    }
}
