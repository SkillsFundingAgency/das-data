using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Statistics.Commands;
using SFA.DAS.Data.Functions.Statistics.Commands.PaymentRdsStatistics;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.StatisticsService
{
    [TestFixture]
    public class WhenCollatePaymentStatisticsMetricsIsInvoked : StatisticsTestsBase
    {
        [Test]
        public async Task ThenTheHandleMethodOnThePaymentStatisticsHandlerIsInvoked()
        {
            await base.StatsService.CollatePaymentStatisticsMetrics();

            base.PaymentStatsHandler.Verify(o => o.Handle(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsSuccessfulCallsRetrieveEquivalentPaymenttatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();
            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), true);

            await base.StatsService.CollatePaymentStatisticsMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentPaymentStatisticsFromRds(), Times.Once);
        }

        [Test]
        public async Task ThenIfRetrieveEquivalentEasStatisticsFromRdsMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnTheModel();
            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), true);

            base.SetupTheRepositoryToThrowDbException(o => o.RetrieveEquivalentPaymentStatisticsFromRds());

            try
            {
                await base.StatsService.CollatePaymentStatisticsMetrics();
            }
            catch (DbException)
            {

            }

            base.Mediator.Verify(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntCallRetrieveEquivalentEasStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await base.StatsService.CollatePaymentStatisticsMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentPaymentStatisticsFromRds(), Times.Never);
        }

        private void SetupTheRepositoryToReturnTheRdsModel()
        {
            base.StatisticsRepository.Setup(o => o.RetrieveEquivalentPaymentStatisticsFromRds())
                .ReturnsAsync(new RdsStatisticsForPaymentsModel());
        }

        private void SetupTheHandlerToReturnHttpRequestException()
        {
            base.PaymentStatsHandler.Setup(o => o.Handle()).Throws<HttpRequestException>();
        }

        private void SetupTheHandlerToReturnTheModel()
        {
            base.PaymentStatsHandler.Setup(o => o.Handle()).ReturnsAsync(new PaymentStatisticsModel());
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await base.StatsService.CollatePaymentStatisticsMetrics();

            base.Mediator.Verify(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenTheMediatorIsInvoked()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), true);

            await base.StatsService.CollatePaymentStatisticsMetrics();

            base.Mediator.Verify(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), Times.Once);
        }

        [Test]
        public async Task ThenIfTheOperationIsSuccessfulAMessageOfTypePaymentsProcessingCompletedMessageIsReturned()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            base.SetupMediatorToReturnResponseOf(o => o.SendAsync<PaymentRdsStatisticsCommandResponse>(It.IsAny<PaymentRdsStatisticsCommand>()), true);

            var actual = await base.StatsService.CollatePaymentStatisticsMetrics();

            Assert.IsAssignableFrom<PaymentsProcessingCompletedMessage>(actual);
        }
    }
}
