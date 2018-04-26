using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Functions.Statistics;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.UnitTests.Statistics
{
    [TestFixture]
    public class WhenTheGetCommitmentStatisticsFunctionIsRun
    {
        private Mock<IStatisticsService> _statsService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILog>();
            _statsService = new Mock<IStatisticsService>();
        }

        [Test]
        public async Task ThenTheStatisticsServiceCollateCommitmentStatisticsMetricsMethodIsInvoked()
        {
            await InvokeRunMethodOnFunction();

            _statsService.Verify(o => o.CollateCommitmentStatisticsMetrics(), Times.Once);
        }

        private async Task InvokeRunMethodOnFunction()
        {
            await GetCommitmentStatisticsFunction.Run(null, _logger.Object, _statsService.Object);
        }
    }
}
