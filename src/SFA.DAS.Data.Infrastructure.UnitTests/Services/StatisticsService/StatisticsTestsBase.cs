using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
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

        [SetUp]
        public void Setup()
        {
            Log = new Mock<ILog>();
            EasStatsHandler = new Mock<IEasStatisticsHandler>();
            StatisticsRepository = new Mock<IStatisticsRepository>();
            Mediator = new Mock<IMediator>();
            EventsApi = new Mock<IEventsApi>();
            CommitmentsStatsHandler = new Mock<ICommitmentsStatisticsHandler>();

            StatsService = new Infrastructure.Services.StatisticsService(
                Log.Object,
                EasStatsHandler.Object,
                StatisticsRepository.Object,
                Mediator.Object,
                EventsApi.Object, 
                CommitmentsStatsHandler.Object);
        }

    }
}
