using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateDataLock;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateDataLocksTests
{
    [TestFixture]
    public class WhenICreateADataLock
    {
        private CreateDataLockCommandHandler _handler;
        private CreateDataLockCommand _command;
        private Mock<IDataLockRepository> _dataLockRepository;
        private Mock<IProviderEventService> _providerEventService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _command = new CreateDataLockCommand {  };

            _dataLockRepository = new Mock<IDataLockRepository>();
            _providerEventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();
            _handler = new CreateDataLockCommandHandler(_dataLockRepository.Object, _providerEventService.Object, _logger.Object);
        }

        [Test]
        public async Task AndThereIsASinglePageOfInformationThenTheDataLocksAreCreated()
        {
            var dataLocks = new PageOfResults<DataLockEvent>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new[] { new DataLockEvent(), new DataLockEvent(), new DataLockEvent() }
            };
            _providerEventService.Setup(x => x.GetDataLocks(1)).ReturnsAsync(dataLocks);

            await _handler.Handle(_command);

            _dataLockRepository.Verify(x => x.SaveDataLocks(dataLocks.Items), Times.Once());
        }

        [Test]
        public async Task AndThereAreMultiplePagesOfInformationThenTheDataLocksAreCreated()
        {
            var dataLocksPage1 = new PageOfResults<DataLockEvent>
            {
                PageNumber = 1,
                TotalNumberOfPages = 3,
                Items = new[] { new DataLockEvent(), new DataLockEvent(), new DataLockEvent() }
            };
            var dataLocksPage2 = new PageOfResults<DataLockEvent>
            {
                PageNumber = 2,
                TotalNumberOfPages = 3,
                Items = new[] { new DataLockEvent(), new DataLockEvent() }
            };
            var dataLocksPage3 = new PageOfResults<DataLockEvent>
            {
                PageNumber = 3,
                TotalNumberOfPages = 3,
                Items = new[] { new DataLockEvent(), new DataLockEvent(), new DataLockEvent() }
            };
            _providerEventService.Setup(x => x.GetDataLocks(1)).ReturnsAsync(dataLocksPage1);
            _providerEventService.Setup(x => x.GetDataLocks(2)).ReturnsAsync(dataLocksPage2);
            _providerEventService.Setup(x => x.GetDataLocks(3)).ReturnsAsync(dataLocksPage3);

            await _handler.Handle(_command);

            _dataLockRepository.Verify(x => x.SaveDataLocks(It.IsAny<IEnumerable<DataLockEvent>>()), Times.Exactly(3));

            _dataLockRepository.Verify(x => x.SaveDataLocks(dataLocksPage1.Items), Times.Once);
            _dataLockRepository.Verify(x => x.SaveDataLocks(dataLocksPage2.Items), Times.Once);
            _dataLockRepository.Verify(x => x.SaveDataLocks(dataLocksPage3.Items), Times.Once);
        }

        [Test]
        public async Task AndGettingDataLocksFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            _providerEventService.Setup(x => x.GetDataLocks(1)).ReturnsAsync(new PageOfResults<DataLockEvent> { PageNumber = 1, TotalNumberOfPages = 3, Items = new DataLockEvent[0] });
            _providerEventService.Setup(x => x.GetDataLocks(2)).ThrowsAsync(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(_command));

            _logger.Verify(x => x.Error(expectedException, $"Exception thrown getting data locks page 2."));
        }

        [Test]
        public async Task AndSavingADataLockFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            var failingDataLock = new DataLockEvent();
            var dataLocks = new PageOfResults<DataLockEvent>
            {
                PageNumber = 1,
                TotalNumberOfPages = 1,
                Items = new[] { new DataLockEvent(), failingDataLock, new DataLockEvent() }
            };
            _providerEventService.Setup(x => x.GetDataLocks(1)).ReturnsAsync(dataLocks);
            _dataLockRepository.Setup(x => x.SaveDataLocks(dataLocks.Items)).Throws(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(_command));

            _logger.Verify(x => x.Error(expectedException, $"Exception thrown saving data locks"));
        }
    }
}
