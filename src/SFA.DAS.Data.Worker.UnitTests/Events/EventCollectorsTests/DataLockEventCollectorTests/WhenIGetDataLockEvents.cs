using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Worker.Events.EventsCollectors;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventCollectorsTests.DataLockEventCollectorTests
{
    public class WhenIGetDataLockEvents
    {
        private Mock<IProviderEventService> _eventService;
        private Mock<ILog> _logger;
        private DataLockEventCollector _collector;
        private List<DataLockEvent> _expectedDataLocks;

        [SetUp]
        public void Arrange()
        {
            _eventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();

            var dataLock = new DataLockEvent { Id = 1 };
            var _expectedDataLocks = new PageOfResults<DataLockEvent>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new DataLockEvent[1] { dataLock }
            };


            _eventService.Setup(x => x.GetUnprocessedDataLocks()).ReturnsAsync(_expectedDataLocks);

            _collector = new DataLockEventCollector(_eventService.Object, _logger.Object, new DataConfiguration { DataLocksEnabled = true });
        }

        [Test]
        public async Task ThenShouldGetEventsFromService()
        {
            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsAssignableFrom<DataLockEvent>(result.First());
            Assert.AreEqual(1, result.First().Id);
        }

        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfNoEventsFound()
        {
            //Arrange
            _eventService.Setup(x => x.GetUnprocessedDataLocks()).ReturnsAsync(new PageOfResults<DataLockEvent>());

            //Act
            var result = await _collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);
        }
        
        [Test]
        public async Task ThenShouldReturnEmptyCollectionIfDataLocksAreNotEnabled()
        {
            //Arrange
            var collector = new DataLockEventCollector(_eventService.Object, _logger.Object, new DataConfiguration { DataLocksEnabled = false });

            //Act
            var result = await collector.GetEvents();

            //Assert
            Assert.IsEmpty(result);

            _eventService.Verify(x => x.GetUnprocessedDataLocks(), Times.Never);
        }
    }
}
