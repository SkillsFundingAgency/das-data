using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;
using SFA.DAS.Data.Tests.Builders;
using SFA.Roatp.Api.Client;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RoatpGatewayTests
{
    public abstract class RoatpGatewayTestsBase
    {
        protected RoatpGateway RoatpGateway;
        protected Mock<IRoatpClient> RoatpClient;
        protected IEnumerable<Roatp.Api.Types.Provider> AllProviders;

        protected readonly string ValidUkPrn = "10007315";

        protected Roatp.Api.Types.Provider ExpectedProvider = new ProviderBuilder().Build();

        [SetUp]
        public void Arrange()
        {
            RoatpClient = new Mock<IRoatpClient>();
            RoatpGateway = new RoatpGateway(RoatpClient.Object);

            RoatpClient.Setup(y => y.Get(ValidUkPrn)).Returns(ExpectedProvider);
            RoatpClient.Setup(y => y.Exists(ValidUkPrn)).Returns(true);
        }
    }
}
