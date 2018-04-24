using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.UnitTests
{
    [TestFixture]
    public class WhenTheGetAccountStatisticsFunctionIsRun
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
        public async Task ThenTheEasStatisticsServiceCollateEasMetricsMethodIsInvoked()
        {
            await InvokeRunMethodOnFunction();

            _statsService.Verify(o => o.CollateEasMetrics(), Times.Once);
        }

        private async Task InvokeRunMethodOnFunction()
        {
            await GetAccountStatisticsFunction.Run(null, _logger.Object, _statsService.Object);
        }
    }
}
