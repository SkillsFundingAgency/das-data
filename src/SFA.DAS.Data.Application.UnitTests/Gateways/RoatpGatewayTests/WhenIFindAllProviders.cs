using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    [TestFixture]
    public class WhenIFindAllProviders : RoatpGatewayTestsBase
    {
        [Test]
        public void ReturnsListOfProviders()
        {
            var allProviders = RoatpGateway.FindAllProviders();

            var enumerable = allProviders as Roatp.Api.Types.Provider[] ?? allProviders.ToArray();

            enumerable.Should().NotBeEmpty();
            enumerable.Should().AllBeOfType<Roatp.Api.Types.Provider>();
            Assert.AreEqual(allProviders, AllProviders);
        }
    }
}
