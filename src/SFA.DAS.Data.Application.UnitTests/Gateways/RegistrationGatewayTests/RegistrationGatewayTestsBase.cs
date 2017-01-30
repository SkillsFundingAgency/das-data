using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Gateways;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.RegistrationGatewayTests
{
    public abstract class RegistrationGatewayTestsBase
    {
        protected AccountGateway AccountGateway;
        protected Mock<IAccountApiClient> AccountApiClient;

        [SetUp]
        public void Arrange()
        {
            AccountApiClient = new Mock<IAccountApiClient>();

            AccountGateway = new AccountGateway(AccountApiClient.Object);
        }
    }
}
