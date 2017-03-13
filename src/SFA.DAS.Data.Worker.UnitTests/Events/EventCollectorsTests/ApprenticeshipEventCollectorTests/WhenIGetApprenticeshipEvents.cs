using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;
using ApprenticeshipEvent = SFA.DAS.Data.Domain.Models.ApprenticeshipEvent;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.ApprenticeshipEventCollectorTests
{
    public class WhenIGetApprenticeshipEvents
    {
        private Mock<IEventService> _eventService;
        private ApprenticeshipEventView _apprenticeshipViewEvent;
        private Mock<ILog> _logger;
        private ApprenticeshipEventsCollector _collector;
        private Mock<IMapper> _mapper;
        private ApprenticeshipEvent _apprenticeshipEvent;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipEvent = new ApprenticeshipEvent();
            _apprenticeshipViewEvent = new ApprenticeshipEventView();

            _eventService = new Mock<IEventService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILog>();

            _eventService.Setup(x => x.GetUnprocessedApprenticeshipEvents())
                         .ReturnsAsync(new List<ApprenticeshipEventView> { _apprenticeshipViewEvent });

            _mapper.Setup(x => x.Map<ApprenticeshipEvent>(It.IsAny<ApprenticeshipEventView>()))
                   .Returns(_apprenticeshipEvent);

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
        public async Task ThenShouldReturnEmptyCollectionIfNoEventsFound()
        {
            //Arrange
            _eventService.Setup(x => x.GetUnprocessedApprenticeshipEvents())
                         .ReturnsAsync(new List<ApprenticeshipEventView> ());

            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);
            _mapper.Verify(x => x.Map<ApprenticeshipEvent>(_apprenticeshipEvent), Times.Never);
        }
    }
}
