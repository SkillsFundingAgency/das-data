using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.StatisticsService
{
    [TestFixture]
    public class WhenCollateEasMetricsIsInvoked
    {
        private IStatisticsService _statsService;
        private Mock<ILog> _log;
        private Mock<IEasStatisticsHandler> _easStatsHandler;
        private Mock<IStatisticsRepository> _statisticsRepository;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _easStatsHandler = new Mock<IEasStatisticsHandler>();
            _statisticsRepository = new Mock<IStatisticsRepository>();

            _statsService = new Infrastructure.Services.StatisticsService(_log.Object, _easStatsHandler.Object, _statisticsRepository.Object);
        }

        [Test]
        public async Task ThenTheHandleMethodOnTheEasStatisticsHandlerIsInvoked()
        {
            await _statsService.CollateEasMetrics();

            _easStatsHandler.Verify(o => o.Handle(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsSuccessfulCallsRetrieveEquivalentEasStatisticsFromRdsOnTheRepository()
        {
            await _statsService.CollateEasMetrics();

            _statisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsSuccessfulCallsSaveEasStatisticsOnTheRepository()
        {
            await _statsService.CollateEasMetrics();

            _statisticsRepository.Verify(o => o.SaveEasStatistics(It.IsAny<EasStatisticsModel>(), It.IsAny<RdsStatisticsForEasModel>()), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntCallRetrieveEquivalentEasStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await _statsService.CollateEasMetrics();

            _statisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Never);
        }

        private void SetupTheHandlerToReturnHttpRequestException()
        {
            _easStatsHandler.Setup(o => o.Handle()).Throws<HttpRequestException>();
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntCallSaveEasStatisticsOnTheRepository()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await _statsService.CollateEasMetrics();

            _statisticsRepository.Verify(o => o.SaveEasStatistics(It.IsAny<EasStatisticsModel>(), It.IsAny<RdsStatisticsForEasModel>()), Times.Never);
        }
    }
}
