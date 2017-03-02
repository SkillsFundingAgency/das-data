using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.EventCollectorsTests.ApprenticeshipEventCollectorTests
{
    public class WhenIGetApprenticeshipEvents
    {
        private Mock<IEventService> _eventService;
        private ApprenticeshipEventView _event;
        private Mock<ILogger> _logger;
        private ApprenticeshipEventsCollector _collector;
        private Mock<IMapper> _mapper;
        private CommitmentsApprenticeshipEvent _commitmentEvent;

        [SetUp]
        public void Arrange()
        {
            _event = new ApprenticeshipEventView();
            _commitmentEvent = new CommitmentsApprenticeshipEvent();

            _eventService = new Mock<IEventService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger>();

            _eventService.Setup(x => x.GetUnprocessedApprenticeshipEvents())
                         .ReturnsAsync(new List<ApprenticeshipEventView> { _event });

            _mapper.Setup(x => x.Map<CommitmentsApprenticeshipEvent>(It.IsAny<ApprenticeshipEventView>()))
                   .Returns(_commitmentEvent);

            _collector = new ApprenticeshipEventsCollector(_eventService.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public async Task ThenShouldGetEventsFromService()
        {
            //Act
            await _collector.GetEvents();

            //Assert
            _eventService.Verify(x => x.GetUnprocessedApprenticeshipEvents(), Times.Once);
        }

        [Test]
        public async Task ThenShouldReturnMappedEvent()
        {
            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.AreEqual(_commitmentEvent, result.FirstOrDefault());
            _mapper.Verify(x => x.Map<CommitmentsApprenticeshipEvent>(_event), Times.Once);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfNoEventsFound()
        {
            //Arrange
            _eventService.Setup(x => x.GetUnprocessedApprenticeshipEvents())
                         .ReturnsAsync(new List<ApprenticeshipEventView> ());

            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);
            _mapper.Verify(x => x.Map<CommitmentsApprenticeshipEvent>(_event), Times.Never);
        }
    }
}
