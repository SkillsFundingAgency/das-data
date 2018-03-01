using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.UnitTests.EasStatisticsHandler
{
    [TestFixture]
    public class WhenTheHandlerIsConstructed
    {
        [Test]
        public void IfTheHttpClientWrapperIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Application.Handlers.EasStatisticsHandler(null, new Mock<IDataConfiguration>().Object, new Mock<ILog>().Object);
            });
        }

        [Test]
        public void IfTheDataConfigurationIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Application.Handlers.EasStatisticsHandler(new Mock<IHttpClientWrapper>().Object, null, new Mock<ILog>().Object);
            });
        }

        [Test]
        public void IfTheLoggerIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Application.Handlers.EasStatisticsHandler(new Mock<IHttpClientWrapper>().Object, new Mock<IDataConfiguration>().Object, null);
            });
        }
    }
}
