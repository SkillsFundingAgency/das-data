using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.AgreementEventCollectorTests
{
    public class WhenIGetAgreementEvents
    {
        private Mock<IEventService> _eventService;
        private AgreementEventView _agreementViewEvent;
        private Mock<ILog> _logger;
        private AgreementEventCollector _collector;

        [SetUp]
        public void Arrange()
        {
            _agreementViewEvent = new AgreementEventView();

            _eventService = new Mock<IEventService>();
            _logger = new Mock<ILog>();

            _eventService.Setup(x => x.GetUnprocessedAgreementEvents())
                .ReturnsAsync(new List<AgreementEventView> {_agreementViewEvent});

            _collector = new AgreementEventCollector(_eventService.Object, _logger.Object);
        }

        [Test]
        public async Task ThenShouldGetEventsFromService()
        {
            await _collector.GetEvents();

            _eventService.Verify(x=>x.GetUnprocessedAgreementEvents(), Times.Once);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfNoEventsFound()
        {
            // Arrange
            _eventService.Setup(x => x.GetUnprocessedAgreementEvents()).ReturnsAsync(new List<AgreementEventView>());

            // Act
            var result = await _collector.GetEvents();

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
