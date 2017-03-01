using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.Data.Worker.Factories;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.EventCollectorsTests.GenericEventCollectorTests
{
    public class WhenGenericEventsAreRequested
    {
        private Mock<IEventService> _eventService;
        private Mock<IEventModelFactory> _eventModelFactory;
        private Mock<ILogger> _logger;
        private GenericEventCollector<TestEvent> _collector;

        private GenericEvent _event;
        private TestEvent _eventModel;

        [SetUp]
        public void Arrange()
        {
            _event = new GenericEvent
            {
                Type = "TestEvent",
                Payload = "{\"Data\":\"test\"}"
            };

            _eventModel = new TestEvent
            {
                Data = "test"
            };

            _eventService = new Mock<IEventService>();
            _eventModelFactory = new Mock<IEventModelFactory>();
            _logger = new Mock<ILogger>();

            _eventService.Setup(x => x.GetGenericEvents(It.IsAny<string>()))
                         .ReturnsAsync(new List<GenericEvent> { _event });

            _eventModelFactory.Setup(x => x.Create<TestEvent>(It.IsAny<string>()))
                              .Returns(_eventModel);

            _collector = new GenericEventCollector<TestEvent>(_eventService.Object, _eventModelFactory.Object, _logger.Object);
        }

        [Test]
        public async Task ThenShouldGetEventsFromService()
        {
            //Act
           await _collector.GetEvents();

            //Assert
            _eventService.Verify(x => x.GetGenericEvents(typeof(TestEvent).Name), Times.Once);
        }

        [Test]
        public async Task ThenShouldReturnTypedEvents()
        {
            //Act
            var events = await _collector.GetEvents();

            //Assert
            _eventModelFactory.Verify(x => x.Create<TestEvent>(_event.Payload), Times.Once);
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(_eventModel, events.FirstOrDefault());
        }

        internal class TestEvent
        {
            public string Data { get; set; }
        }
    }
}
