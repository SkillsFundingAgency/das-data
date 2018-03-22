using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.UnitTests
{
    [TestFixture]
    public class WhenTheGetPaymentsStatisticsFunctionIsRun
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
        public async Task ThenTheStatisticsServiceCollatePaymentStatisticsMetricsMethodIsInvoked()
        {
            await InvokeRunMethodOnFunction();

            _statsService.Verify(o => o.CollatePaymentStatisticsMetrics(), Times.Once);
        }

        private async Task InvokeRunMethodOnFunction()
        {
            await GetPaymentsStatisticsFunction.Run(null, _logger.Object, _statsService.Object);
        }
    }
}
