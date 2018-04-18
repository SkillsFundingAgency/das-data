using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.StatisticsService
{
    public abstract class StatisticsTestsBase
    {
        protected IStatisticsService StatsService;
        protected Mock<ILog> Log;
        protected Mock<IEasStatisticsHandler> EasStatsHandler;
        protected Mock<IStatisticsRepository> StatisticsRepository;
        protected Mock<IMediator> Mediator;
        protected Mock<IEventsApi> EventsApi;
        protected Mock<ICommitmentsStatisticsHandler> CommitmentsStatsHandler;
        protected Mock<IPaymentStatisticsHandler> PaymentStatsHandler;

        [SetUp]
        public void Setup()
        {
            Log = new Mock<ILog>();
            EasStatsHandler = new Mock<IEasStatisticsHandler>();
            StatisticsRepository = new Mock<IStatisticsRepository>();
            Mediator = new Mock<IMediator>();
            CommitmentsStatsHandler = new Mock<ICommitmentsStatisticsHandler>();
            PaymentStatsHandler = new Mock<IPaymentStatisticsHandler>();

            StatsService = new Infrastructure.Services.StatisticsService(
                Log.Object,
                EasStatsHandler.Object,
                StatisticsRepository.Object,
                Mediator.Object,
                CommitmentsStatsHandler.Object,
                PaymentStatsHandler.Object);
        }

        protected void SetupTheRepositoryToThrowDbException<T>(Expression<Func<IStatisticsRepository, Task<T>>> expression)
        {
            StatisticsRepository.Setup(expression)
                .ThrowsAsync(new Mock<DbException>().Object);
        }

        protected void SetupMediatorToReturnResponseOf<T>(Expression<Func<IMediator, Task<T>>> expression, bool successful) where T : ICommandResponse, new()
        {
            Mediator.Setup(expression)
                .ReturnsAsync(new T
                {
                    OperationSuccessful = successful
                });
        }
    }
}
