using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    [TestFixture]
    public class WhenICheckIfProviderExists : RoatpGatewayTestsBase
    {
        [Test]
        public void WithNullUkPrnReturnsFalse()
        {
            var exists = RoatpGateway.ProviderExists(null);
            Assert.IsFalse(exists);
        }

        [Test]
        public void WithValidUkPrnReturnsTrue()
        {
            var exists = RoatpGateway.ProviderExists(ValidUkPrn);
            Assert.IsTrue(exists);
        }
    }
}
