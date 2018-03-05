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
                new Infrastructure.Services.StatisticsService(null, new Mock<IEasStatisticsHandler>().Object,
                    new Mock<IStatisticsRepository>().Object, new Mock<IMediator>().Object);
            });
        }

        [Test]
        public void ThenIfEasStatisticsHandlerIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Infrastructure.Services.StatisticsService(new Mock<ILog>().Object, null,
                    new Mock<IStatisticsRepository>().Object, new Mock<IMediator>().Object);
            });
        }

        [Test]
        public void ThenIfStatisticsRepositoryIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Infrastructure.Services.StatisticsService(new Mock<ILog>().Object, new Mock<IEasStatisticsHandler>().Object,
                    null, new Mock<IMediator>().Object);
            });
        }

        [Test]
        public void ThenIfTheMediatorIsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Infrastructure.Services.StatisticsService(new Mock<ILog>().Object, new Mock<IEasStatisticsHandler>().Object,
                    new Mock<IStatisticsRepository>().Object, null);
            });
        }
    }
}
