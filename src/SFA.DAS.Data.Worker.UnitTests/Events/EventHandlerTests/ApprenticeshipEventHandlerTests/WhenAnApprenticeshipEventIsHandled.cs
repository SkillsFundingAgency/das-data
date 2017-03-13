using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;
using ApprenticeshipEvent = SFA.DAS.Data.Domain.Models.ApprenticeshipEvent;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.ApprenticeshipEventHandlerTests
{
    public class WhenAnApprenticeshipEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private ApprenticeshipEventHandler _handler;
        private Mock<IMapper> _mapper;
        private ApprenticeshipEvent _event;
        private Mock<IDataConfiguration> _configuration;
        private Mock<IEventRepository> _eventRepository;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _eventRepository = new Mock<IEventRepository>();
            _logger = new Mock<ILog>();
            _configuration = new Mock<IDataConfiguration>();

            _configuration.SetupGet(x => x.FailureTolerance).Returns(5);


            _handler = new ApprenticeshipEventHandler(
                _mediator.Object, _mapper.Object, _eventRepository.Object,
                _configuration.Object, _logger.Object);

            _event = new ApprenticeshipEvent();

            _mediator.Setup(x => x.SendAsync(It.IsAny<CreateCommitmentApprenticeshipEntryCommand>()))
                     .ReturnsAsync(new CreateCommitmentApprenticeshipEntryResponse());

            _mapper.Setup(x => x.Map<ApprenticeshipEvent>(It.IsAny<ApprenticeshipEventView>()))
                .Returns(_event);
        }

        [Test]
        public async Task ThenTheCreateEventCommandShouldBeSent()
        {
            //Act
            await _handler.Handle(new ApprenticeshipEventView());

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<CreateCommitmentApprenticeshipEntryCommand>(
                c => c.Event != null && c.Event.Equals(_event))), Times.Once);
        }
    }
}
