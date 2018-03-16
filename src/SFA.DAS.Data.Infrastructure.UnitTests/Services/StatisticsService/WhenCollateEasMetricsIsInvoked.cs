using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands.EasRdsStatistics;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.StatisticsService
{
    [TestFixture]
    public class WhenCollateEasMetricsIsInvoked : StatisticsTestsBase
    {
        [Test]
        public async Task ThenTheHandleMethodOnTheEasStatisticsHandlerIsInvoked()
        {
            await base.StatsService.CollateEasMetrics();

            base.EasStatsHandler.Verify(o => o.Handle(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsSuccessfulCallsRetrieveEquivalentEasStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();
            SetupMediatorToReturnResponseOf(true);

            await base.StatsService.CollateEasMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Once);
        }

        [Test]
        public async Task ThenIfRetrieveEquivalentEasStatisticsFromRdsMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnTheModel();
            SetupMediatorToReturnResponseOf(true);
            SetupTheRepositoryToThrowDbException();

            try
            {
                await base.StatsService.CollateEasMetrics();
            }
            catch (DbException)
            {

            }

            base.Mediator.Verify(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntCallRetrieveEquivalentEasStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await base.StatsService.CollateEasMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Never);
        }

        private void SetupTheRepositoryToReturnTheRdsModel()
        {
            base.StatisticsRepository.Setup(o => o.RetrieveEquivalentEasStatisticsFromRds())
                .ReturnsAsync(new RdsStatisticsForEasModel());
        }

        private void SetupTheHandlerToReturnHttpRequestException()
        {
            base.EasStatsHandler.Setup(o => o.Handle()).Throws<HttpRequestException>();
        }

        private void SetupTheHandlerToReturnTheModel()
        {
            base.EasStatsHandler.Setup(o => o.Handle()).ReturnsAsync(new EasStatisticsModel());
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await base.StatsService.CollateEasMetrics();

            base.Mediator.Verify(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenTheMediatorIsInvoked()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            SetupMediatorToReturnResponseOf(true);

            await base.StatsService.CollateEasMetrics();

            base.Mediator.Verify(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), Times.Once);
        }

        [Test]
        public async Task ThenIfTheOperationIsSuccessfulAMessageIsAddedToTheEventsApi()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            SetupMediatorToReturnResponseOf(true);

            await base.StatsService.CollateEasMetrics();

            base.EventsApi.Verify(o => o.CreateGenericEvent(It.IsAny<GenericEvent>()), Times.Once);
        }

        [Test]
        public async Task ThenIfTheOperationIsNotSuccessfulNoMessageIsAddedToTheEventsApi()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            SetupMediatorToReturnResponseOf(false);

            await base.StatsService.CollateEasMetrics();

            base.EventsApi.Verify(o => o.CreateGenericEvent(It.IsAny<GenericEvent>()), Times.Never);
        }

        private void SetupMediatorToReturnResponseOf(bool successful)
        {
            base.Mediator.Setup(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()))
                .ReturnsAsync(new EasRdsStatisticsCommandResponse()
                {
                    OperationSuccessful = successful
                });
        }

        private void SetupTheRepositoryToThrowDbException()
        {
            base.StatisticsRepository.Setup(o => o.RetrieveEquivalentEasStatisticsFromRds())
                .ThrowsAsync(new Mock<DbException>().Object);
        }
    }
}
