using System;
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
        private Mock<IDataLockRepository> _dataLockRepository;
        private Mock<IProviderEventService> _providerEventService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _dataLockRepository = new Mock<IDataLockRepository>();
            _providerEventService = new Mock<IProviderEventService>();
            _logger = new Mock<ILog>();
            _handler = new CreateDataLockCommandHandler(_dataLockRepository.Object, _providerEventService.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheDataLockIsSaved()
        {
            var dataLock = new DataLockEvent { Id = 1 };

            await _handler.Handle(new CreateDataLockCommand { Event = dataLock });

            _dataLockRepository.Verify(x => x.SaveDataLock(dataLock), Times.Once);
        }
        
        [Test]
        public async Task AndSavingADataLockFailsThenTheExceptionIsLogged()
        {
            var expectedException = new Exception();
            var failingDataLock = new DataLockEvent();

            var command = new CreateDataLockCommand { Event = failingDataLock };
            _dataLockRepository.Setup(x => x.SaveDataLock(failingDataLock)).Throws(expectedException);

            Assert.ThrowsAsync<Exception>(() => _handler.Handle(command));

            _logger.Verify(x => x.Error(expectedException, $"Exception thrown saving data lock"));
        }
    }
}
