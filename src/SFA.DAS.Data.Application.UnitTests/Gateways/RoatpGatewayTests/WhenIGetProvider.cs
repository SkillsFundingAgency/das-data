using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    [TestFixture]
    public class WhenIGetProvider : RoatpGatewayTestsBase
    {
        [Test]
        public void WithNullUkPrnReturnsNull()
        {
            var x = RoatpGateway.GetProvider(null);
            Assert.IsNull(x);
        }

        [Test]
        public void WithValidUkPrnReturnsProvider()
        {
            var x = RoatpGateway.GetProvider(ValidUkPrn);
            Assert.IsNotNull(x);
            Assert.IsTrue(x.GetType() == typeof(Roatp.Api.Types.Provider));
        }
    }
}
