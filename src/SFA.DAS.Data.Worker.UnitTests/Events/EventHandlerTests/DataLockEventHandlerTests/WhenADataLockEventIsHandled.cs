using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateDataLock;
using SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.DataLockEventHandlerTests
{
    public class WhenADataLockEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private DataLockEventHandler _handler;
        private Mock<IDataConfiguration> _configuration;
        private Mock<IEventRepository> _eventRepository;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _eventRepository = new Mock<IEventRepository>();
            _logger = new Mock<ILog>();
            _configuration = new Mock<IDataConfiguration>();

            _configuration.SetupGet(x => x.FailureTolerance).Returns(5);
            
            _handler = new DataLockEventHandler(_mediator.Object, _eventRepository.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheCreateEventCommandShouldBeSent()
        {
            //Arrange
            var dataLockEvent = new DataLockEvent { Id = 123 };

            //Act
            await _handler.Handle(dataLockEvent);

            //Assert
            _mediator.Verify(x => x.PublishAsync(It.IsAny<CreateDataLockCommand>()), Times.Once);
            _mediator.Verify(x => x.PublishAsync(It.Is<CreateDataLockCommand>(c => c.Event == dataLockEvent)), Times.Once);

            Assert.Inconclusive();
            Assert.Fail("Mikey");
        }
    }
}
