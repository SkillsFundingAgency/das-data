using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateEmploymentCheck;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.EmploymentCheck.Events;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.EmploymentCheckCompleteEventHandlerTests
{
    public class WhenAnEmploymentCheckCompleteEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private EmploymentCheckCompleteEventHandler _handler;
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


            _handler = new EmploymentCheckCompleteEventHandler(_mediator.Object, _eventRepository.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheCreateEmploymentCheckCommandShouldBeSent()
        {
            //Arrange
            var employmentCheck = new GenericEvent<EmploymentCheckCompleteEvent>
            {
                Payload = new EmploymentCheckCompleteEvent()
            };

            //Act
            await _handler.Handle(employmentCheck);

            //Assert
            _mediator.Verify(x => x.PublishAsync(It.Is<CreateEmploymentCheckCommand>(c => c.Event == employmentCheck.Payload)), Times.Once);
        }
    }
}
