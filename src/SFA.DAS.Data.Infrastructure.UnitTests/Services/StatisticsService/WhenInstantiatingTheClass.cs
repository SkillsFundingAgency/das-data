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
    [TestFixture]
    public class WhenInstantiatingTheClass
    {
        [Test]
        public void ThenIfLogIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Functions.Statistics.Services.StatisticsService(null, new Mock<IEasStatisticsHandler>().Object,
                    new Mock<IStatisticsRepository>().Object, new Mock<IMediator>().Object,  new Mock<ICommitmentsStatisticsHandler>().Object, new Mock<IPaymentStatisticsHandler>().Object);
            });
        }

        [Test]
        public void ThenIfEasStatisticsHandlerIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Functions.Statistics.Services.StatisticsService(new Mock<ILog>().Object, null,
                    new Mock<IStatisticsRepository>().Object, new Mock<IMediator>().Object,  new Mock<ICommitmentsStatisticsHandler>().Object, new Mock<IPaymentStatisticsHandler>().Object);
            });
        }

        [Test]
        public void ThenIfStatisticsRepositoryIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Functions.Statistics.Services.StatisticsService(new Mock<ILog>().Object, new Mock<IEasStatisticsHandler>().Object,
                    null, new Mock<IMediator>().Object,  new Mock<ICommitmentsStatisticsHandler>().Object, new Mock<IPaymentStatisticsHandler>().Object);
            });
        }

        [Test]
        public void ThenIfTheMediatorIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Functions.Statistics.Services.StatisticsService(new Mock<ILog>().Object, new Mock<IEasStatisticsHandler>().Object,
                    new Mock<IStatisticsRepository>().Object, null,  new Mock<ICommitmentsStatisticsHandler>().Object, new Mock<IPaymentStatisticsHandler>().Object);
            });
        }

        [Test]
        public void ThenIfTheCommitmentsStatisticsHandlerIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Functions.Statistics.Services.StatisticsService(new Mock<ILog>().Object, new Mock<IEasStatisticsHandler>().Object,
                    new Mock<IStatisticsRepository>().Object, new Mock<IMediator>().Object,  null, new Mock<IPaymentStatisticsHandler>().Object);
            });
        }

        [Test]
        public void ThenIfThePaymentStatisticsHandlerIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Functions.Statistics.Services.StatisticsService(new Mock<ILog>().Object, new Mock<IEasStatisticsHandler>().Object,
                    new Mock<IStatisticsRepository>().Object, new Mock<IMediator>().Object,  new Mock<ICommitmentsStatisticsHandler>().Object, null);
            });
        }
    }
}
