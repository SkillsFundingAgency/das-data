using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    [TestFixture]
    public class WhenIGetProvider : RoatpGatewayTestsBase
    {
        [Test]
        public void WithNullUkPrnReturnsNull()
        {
            var provider = RoatpGateway.GetProvider(null);
            Assert.IsNull(provider);
        }

        [Test]
        public void WithValidUkPrnReturnsProvider()
        {
            var provider = RoatpGateway.GetProvider(ValidUkPrn);
            Assert.IsNotNull(provider);
            Assert.AreSame(provider, ExpectedProvider);
        }
    }
}
