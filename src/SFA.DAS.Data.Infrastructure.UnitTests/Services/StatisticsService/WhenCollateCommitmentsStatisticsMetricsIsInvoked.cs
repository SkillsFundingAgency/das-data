using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions;
using SFA.DAS.Data.Functions.Commands.CommitmentRdsStatistics;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.StatisticsService
{
    [TestFixture]
    public class WhenCollateCommitmentsStatisticsMetricsIsInvoked : StatisticsTestsBase
    {
        [Test]
        public async Task ThenTheHandleMethodOnTheCommitmentstatisticsHandlerIsInvoked()
        {
            await base.StatsService.CollateCommitmentStatisticsMetrics();

            base.CommitmentsStatsHandler.Verify(o => o.Handle(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsSuccessfulCallsRetrieveEquivalentCommitmentStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();
            SetupMediatorToReturnResponseOf(true);

            await base.StatsService.CollateCommitmentStatisticsMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentCommitmentsStatisticsFromRds(), Times.Once);
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntCallRetrieveEquivalentCommitmentStatisticsFromRdsOnTheRepository()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await base.StatsService.CollateCommitmentStatisticsMetrics();

            base.StatisticsRepository.Verify(o => o.RetrieveEquivalentCommitmentsStatisticsFromRds(), Times.Never);
        }

        private void SetupTheRepositoryToReturnTheRdsModel()
        {
            base.StatisticsRepository.Setup(o => o.RetrieveEquivalentCommitmentsStatisticsFromRds())
                .ReturnsAsync(new RdsStatisticsForCommitmentsModel());
        }

        private void SetupTheHandlerToReturnHttpRequestException()
        {
            base.CommitmentsStatsHandler.Setup(o => o.Handle()).Throws<HttpRequestException>();
        }

        private void SetupTheRepositoryToThrowDbException()
        {
            base.StatisticsRepository.Setup(o => o.RetrieveEquivalentCommitmentsStatisticsFromRds())
                .ThrowsAsync(new Mock<DbException>().Object);
        }

        private void SetupTheHandlerToReturnTheModel()
        {
            base.CommitmentsStatsHandler.Setup(o => o.Handle()).ReturnsAsync(new CommitmentsStatisticsModel());
        }

        [Test]
        public async Task ThenIfHandleMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnHttpRequestException();
            await base.StatsService.CollateCommitmentStatisticsMetrics();

            base.Mediator.Verify(o => o.SendAsync<CommitmentRdsStatisticsCommandResponse>(It.IsAny<CommitmentRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenIfRetrieveEquivalentCommitmentsStatisticsFromRdsMethodIsNotSuccessfulItDoesntInvokeTheMediator()
        {
            SetupTheHandlerToReturnTheModel();
            SetupMediatorToReturnResponseOf(true);
            SetupTheRepositoryToThrowDbException();

            try
            {
                await base.StatsService.CollateCommitmentStatisticsMetrics();
            }
            catch (DbException e)
            {

            }
           
            base.Mediator.Verify(o => o.SendAsync<CommitmentRdsStatisticsCommandResponse>(It.IsAny<CommitmentRdsStatisticsCommand>()), Times.Never);
        }

        [Test]
        public async Task ThenTheMediatorIsInvoked()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            SetupMediatorToReturnResponseOf(true);

            await base.StatsService.CollateCommitmentStatisticsMetrics();

            base.Mediator.Verify(o => o.SendAsync<CommitmentRdsStatisticsCommandResponse>(It.IsAny<CommitmentRdsStatisticsCommand>()), Times.Once);
        }

        [Test]
        public async Task ThenIfTheOperationIsSuccessfulAMessageOfTypeCommitmentProcessingCompletedMessageIsReturned()
        {
            SetupTheHandlerToReturnTheModel();
            SetupTheRepositoryToReturnTheRdsModel();

            SetupMediatorToReturnResponseOf(true);

            var actual = await base.StatsService.CollateCommitmentStatisticsMetrics();

           Assert.IsAssignableFrom<CommitmentProcessingCompletedMessage>(actual);
        }

        private void SetupMediatorToReturnResponseOf(bool successful)
        {
            base.Mediator.Setup(o => o.SendAsync<CommitmentRdsStatisticsCommandResponse>(It.IsAny<CommitmentRdsStatisticsCommand>()))
                .ReturnsAsync(new CommitmentRdsStatisticsCommandResponse()
                {
                    OperationSuccessful = successful
                });
        }
    }
}
