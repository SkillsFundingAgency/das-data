using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Data.Worker.Events;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventWatcherTests
{
    public class WhenIProcessEvents
    {
        private List<IEventsProcessor> _eventsProcessors;
        private Mock<IEventsProcessor> _processor;
        private Mock<ILogger> _logger;
        private EventsWatcher _watcher;

        [SetUp]
        public void Arrange()
        {
            _processor = new Mock<IEventsProcessor>();
            _logger = new Mock<ILogger>();

            _eventsProcessors = new List<IEventsProcessor>
            {
                _processor.Object
            };
            
            _watcher = new EventsWatcher(_eventsProcessors, _logger.Object);
        }

        [Test]
        public async Task ThenEventProcessorsShouldBeCalled()
        {
            //Act
            await _watcher.ProcessEvents();

            //Assert
            _processor.Verify(x => x.ProcessEvents(), Times.Once);
        }

        [Test]
        public async Task ThenIfErrorOccursItShouldBeLogged()
        {
            //Arrange
            _processor.Setup(x => x.ProcessEvents())
                      .Throws<Exception>();

            //Act
            await _watcher.ProcessEvents();

            //Assert
            _logger.Verify(x  => x.Error(It.IsAny<Exception>(), "Error occurred whilst processing events"));
        }

        [Test]
        public async Task ThenIfErrorOccursOtherEventProcessorsShouldStillGetCalled()
        {
            //Arrange
            var anotherProcessor = new Mock<IEventsProcessor>();

            _eventsProcessors.Add(anotherProcessor.Object);

            _processor.Setup(x => x.ProcessEvents())
                      .Throws<Exception>();

            //Act
            await _watcher.ProcessEvents();

            //Assert
            anotherProcessor.Verify(x => x.ProcessEvents(), Times.Once);
        }
    }
}
