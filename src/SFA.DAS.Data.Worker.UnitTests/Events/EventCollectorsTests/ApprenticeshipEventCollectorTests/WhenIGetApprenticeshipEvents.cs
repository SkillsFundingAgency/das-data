using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.ApprenticeshipEventCollectorTests
{
    public class WhenIGetApprenticeshipEvents
    {
        private Mock<IEventService> _eventService;
        private ApprenticeshipEventView _apprenticeshipViewEvent;
        private Mock<ILog> _logger;
        private ApprenticeshipEventsCollector _collector;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipViewEvent = new ApprenticeshipEventView();

            _eventService = new Mock<IEventService>();
            _logger = new Mock<ILog>();

            _eventService.Setup(x => x.GetUnprocessedApprenticeshipEvents())
                         .ReturnsAsync(new List<ApprenticeshipEventView> { _apprenticeshipViewEvent });

            _collector = new ApprenticeshipEventsCollector(_eventService.Object, _logger.Object);
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
        }
    }
}
