﻿using System.Threading.Tasks;
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

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.ApprenticeshipEventHandlerTests
{
    public class WhenAnApprenticeshipEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private ApprenticeshipEventHandler _handler;
        private Mock<IMapper> _mapper;
        private CommitmentsApprenticeshipEvent _commitmentEvent;
        private DataConfiguration _configuration;
        private Mock<IEventRepository> _eventRepository;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _eventRepository = new Mock<IEventRepository>();
            _logger = new Mock<ILogger>();
            _configuration = new DataConfiguration();

            _handler = new ApprenticeshipEventHandler(
                _mediator.Object, _mapper.Object, _eventRepository.Object,
                _configuration, _logger.Object);

            _commitmentEvent = new CommitmentsApprenticeshipEvent();

            _mediator.Setup(x => x.SendAsync(It.IsAny<CreateCommitmentApprenticeshipEntryCommand>()))
                     .ReturnsAsync(new CreateCommitmentApprenticeshipEntryResponse());

            _mapper.Setup(x => x.Map<CommitmentsApprenticeshipEvent>(It.IsAny<ApprenticeshipEventView>()))
                .Returns(_commitmentEvent);
        }

        [Test]
        public async Task ThenTheCreateEventCommandShouldBeSent()
        {
            //Act
            await _handler.Handle(new ApprenticeshipEventView());

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<CreateCommitmentApprenticeshipEntryCommand>(
                c => c.Event != null && c.Event.Equals(_commitmentEvent))), Times.Once);
        }
    }
}
