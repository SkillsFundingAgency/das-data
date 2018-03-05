using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.EasRdsStatistics;
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
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _easStatsHandler = new Mock<IEasStatisticsHandler>();
            _statisticsRepository = new Mock<IStatisticsRepository>();
            _mediator = new Mock<IMediator>();

            _statsService = new Infrastructure.Services.StatisticsService(_log.Object, _easStatsHandler.Object, _statisticsRepository.Object, _mediator.Object);
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
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();
            SetupMediatorToReturnResponseOf(true);

            await _statsService.CollateEasMetrics();

            _statisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntCallRetrieveEquivalentEasStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await _statsService.CollateEasMetrics();

            _statisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Never);
        }

        private void SetupTheRepositoryToReturnTheRdsModel()
        {
            _statisticsRepository.Setup(o => o.RetrieveEquivalentEasStatisticsFromRds())
                .ReturnsAsync(new RdsStatisticsForEasModel());
        }

        private void SetupTheHandlerToReturnHttpRequestException()
        {
            _easStatsHandler.Setup(o => o.Handle()).Throws<HttpRequestException>();
        }

        private void SetupTheHandlerToReturnTheModel()
        {
            _easStatsHandler.Setup(o => o.Handle()).ReturnsAsync(new EasStatisticsModel());
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await _statsService.CollateEasMetrics();

            _mediator.Verify(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenTheMediatorIsInvoked()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            SetupMediatorToReturnResponseOf(true);

            await _statsService.CollateEasMetrics();

            _mediator.Verify(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), Times.Once);
        }

        private void SetupMediatorToReturnResponseOf(bool successful)
        {
            _mediator.Setup(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()))
                .ReturnsAsync(new EasRdsStatisticsCommandResponse()
                {
                    OperationSuccessful = successful
                });
        }
    }
}
