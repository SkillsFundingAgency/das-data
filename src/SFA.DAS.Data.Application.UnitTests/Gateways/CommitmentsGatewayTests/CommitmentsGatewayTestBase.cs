using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Api.Client.Interfaces;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Data.Application.Gateways;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.CommitmentsGatewayTests
{
    public class CommitmentsGatewayTestBase
    {
        protected CommitmentsGateway CommitmentsGateway;
        protected Mock<IStatisticsApi> StatisticsApi;

        protected ConsistencyStatistics ExpectedStatistics = new CommitmentsStatisticsBuilder().Build();

        [SetUp]
        public void Arrange()
        {
            StatisticsApi = new Mock<IStatisticsApi>();
            CommitmentsGateway = new CommitmentsGateway(StatisticsApi.Object);

            StatisticsApi.Setup(x => x.GetStatistics()).ReturnsAsync(ExpectedStatistics);
        }
    }
}
