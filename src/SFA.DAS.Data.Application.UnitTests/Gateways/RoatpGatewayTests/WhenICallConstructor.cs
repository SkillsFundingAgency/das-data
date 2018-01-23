using System;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    [TestFixture]
    public class WhenICallConstructor : RoatpGatewayTestsBase
    {
        [Test]
        public void WithNullRoatpClientThenNullArgumentExceptionRaised()
        {
            Assert.Throws<ArgumentNullException>(() => new RoatpGateway(null));
        }

        [Test]
        public void WithNonNullClientThenDoesNotThrowException()
        {
            Assert.DoesNotThrow(()=>new RoatpGateway(RoatpClient.Object));
        }
    }
}
