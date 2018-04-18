using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Statistics.Commands;
using SFA.DAS.Data.Functions.Statistics.Commands.EasRdsStatistics;

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
            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), true);

            await base.StatsService.CollateEasMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentEasStatisticsFromRds(), Times.Once);
        }

        [Test]
        public async Task ThenIfRetrieveEquivalentEasStatisticsFromRdsMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnTheModel();
            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), true);

            base.SetupTheRepositoryToThrowDbException(o => o.RetrieveEquivalentEasStatisticsFromRds());

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

            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), true);

            await base.StatsService.CollateEasMetrics();

            base.Mediator.Verify(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), Times.Once);
        }

        [Test]
        public async Task ThenIfTheOperationIsSuccessfulAMessageOfTypeEasProcessingCompletedMessageIsReturned()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<EasRdsStatisticsCommandResponse>(It.IsAny<EasRdsStatisticsCommand>()), true);

            var actual = await base.StatsService.CollateEasMetrics();

            Assert.IsAssignableFrom<EasProcessingCompletedMessage>(actual);
        }
    }
}
