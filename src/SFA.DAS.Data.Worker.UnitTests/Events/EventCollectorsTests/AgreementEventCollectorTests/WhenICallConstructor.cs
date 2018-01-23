using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.AgreementEventCollectorTests
{
    public class WhenICallConstructor
    {
        private Mock<ILog> _logger;
        private Mock<IEventService> _eventService;
        private AgreementEventCollector _collector;

        [Test]
        public void WithNullEventServiceThenNullArgumentExceptionRaised()
        {
            _logger = new Mock<ILog>();

            Assert.Throws<ArgumentNullException>(() => new AgreementEventCollector(null, _logger.Object)) ;
        }

        [Test]
        public void WithNullLoggerThenNullArgumentExceptionEaised()
        {
            _eventService = new Mock<IEventService>();

            Assert.Throws<ArgumentNullException>(() => new AgreementEventCollector(_eventService.Object, null));
        }

        [Test]
        public void WithNonNullEventServiceAndLoggerThenDoesNotThrowArgumentNullException()
        {
            _logger = new Mock<ILog>();
            _eventService = new Mock<IEventService>();

            Assert.DoesNotThrow(()=> new AgreementEventCollector(_eventService.Object, _logger.Object));
        }
    }
}
