using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.PaymentEventCollectorTests
{
    public class WhenIGetPaymentEvents
    {
        private Mock<IProviderEventService> _eventService;
        private Mock<ILog> _logger;
        private PeriodEndEventsCollector<Payment> _collector;
        private List<PeriodEnd> _expectedPeriodEnds;

        [SetUp]
        public void Arrange()
        {
            _eventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();

            var periodEnd = new PeriodEnd {Id = "33"};
            _expectedPeriodEnds = new List<PeriodEnd> {periodEnd};

            _eventService.Setup(x => x.GetUnprocessedPeriodEnds<Payment>()).ReturnsAsync(_expectedPeriodEnds);

            _collector = new PaymentEventCollector(_eventService.Object, _logger.Object, new DataConfiguration { PaymentsEnabled = true });
        }

        [Test]
        public async Task ThenShouldGetEventsFromService()
        {
            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsAssignableFrom<PeriodEndEvent<Payment>>(result.First());
            Assert.AreEqual("33", result.First().PeriodEnd.Id);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfNoEventsFound()
        {
            //Arrange
            _eventService.Setup(x => x.GetUnprocessedPeriodEnds<Payment>()).ReturnsAsync(new List<PeriodEnd>());

            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfPaymentsAreNotEnabled()
        {
            //Arrange
            var collector = new PaymentEventCollector(_eventService.Object, _logger.Object, new DataConfiguration { PaymentsEnabled = false });

            //Act
            var result = await collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);

            _eventService.Verify(x => x.GetUnprocessedPeriodEnds<Payment>(), Times.Never);
        }
    }
}

