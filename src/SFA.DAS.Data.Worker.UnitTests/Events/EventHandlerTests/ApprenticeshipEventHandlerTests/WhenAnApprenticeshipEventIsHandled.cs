using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.ApprenticeshipEventHandlerTests
{
    public class WhenAnApprenticeshipEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private ApprenticeshipEventHandler _handler;
        private ApprenticeshipEventView _event;
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


            _handler = new ApprenticeshipEventHandler(
                _mediator.Object, _eventRepository.Object,
                _configuration.Object, _logger.Object);

            _event = new ApprenticeshipEventView();

            _mediator.Setup(x => x.SendAsync(It.IsAny<CreateCommitmentApprenticeshipEntryCommand>()))
                     .ReturnsAsync(new CreateCommitmentApprenticeshipEntryResponse());
        }

        [Test]
        public async Task ThenTheCreateEventCommandShouldBeSent()
        {
            //Act
            await _handler.Handle(_event);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<CreateCommitmentApprenticeshipEntryCommand>(
                c => c.Event != null && c.Event.Equals(_event))), Times.Once);
        }
    }
}
