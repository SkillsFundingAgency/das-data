using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Handlers.CommitmentsStatisticsHandler
{
    [TestFixture]
    public class WhenTheHandlerIsConstructed
    {
        [Test]
        public void IfTheHttpClientWrapperIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Application.Handlers.CommitmentsStatisticsHandler(null, new Mock<IDataConfiguration>().Object, new Mock<ILog>().Object);
            });
        }

        [Test]
        public void IfTheDataConfigurationIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Application.Handlers.CommitmentsStatisticsHandler(new Mock<IHttpClientWrapper>().Object, null, new Mock<ILog>().Object);
            });
        }

        [Test]
        public void IfTheLoggerIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Application.Handlers.CommitmentsStatisticsHandler(new Mock<IHttpClientWrapper>().Object, new Mock<IDataConfiguration>().Object, null);
            });
        }
    }
}
