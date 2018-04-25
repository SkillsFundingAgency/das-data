using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.PaymentEventCollectorTests
{
    public class WhenIGetPaymentEvents
    {
        private Mock<IProviderEventService> _eventService;
        private Mock<ILog> _logger;
        private PeriodEndEventsCollector _collector;
        private List<PeriodEnd> _expectedPeriodEnds;

        [SetUp]
        public void Arrange()
        {
            _eventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();

            _expectedPeriodEnds = new List<PeriodEnd> { new PeriodEnd() };

            _eventService.Setup(x => x.GetUnprocessedPeriodEnds()).ReturnsAsync(_expectedPeriodEnds);

            _collector = new PeriodEndEventsCollector(_eventService.Object, _logger.Object, new DataConfiguration { PaymentsEnabled = true });
        }

        [Test]
        public async Task ThenShouldGetEventsFromService()
        {
            //Act
            var result = await _collector.GetEvents();

            //Assert
            result.Should().BeSameAs(_expectedPeriodEnds);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfNoEventsFound()
        {
            //Arrange
            _eventService.Setup(x => x.GetUnprocessedPeriodEnds()).ReturnsAsync(new List<PeriodEnd>());

            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfPaymentsAreNotEnabled()
        {
            //Arrange
            var collector = new PeriodEndEventsCollector(_eventService.Object, _logger.Object, new DataConfiguration { PaymentsEnabled = false });

            //Act
            var result = await collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);

            _eventService.Verify(x => x.GetUnprocessedPeriodEnds(), Times.Never);
        }
    }
}
