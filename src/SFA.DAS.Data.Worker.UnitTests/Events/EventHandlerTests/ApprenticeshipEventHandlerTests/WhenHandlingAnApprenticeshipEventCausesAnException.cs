﻿using System;
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
    public class WhenHandlingAnApprenticeshipEventCausesAnException
    {
        private Mock<IMediator> _mediator;
        private ApprenticeshipEventHandler _handler;
        private Mock<IDataConfiguration> _configuration;
        private Mock<IEventRepository> _eventRepository;
        private Mock<ILog> _logger;

        private int _eventRetryCount;
        private int _tryCount;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _eventRepository = new Mock<IEventRepository>();
            _logger = new Mock<ILog>();
            _configuration = new Mock<IDataConfiguration>();

            _eventRetryCount = 5;
            _tryCount = 2;

            _configuration.SetupGet(x => x.FailureTolerance).Returns(_eventRetryCount);

            _handler = new ApprenticeshipEventHandler(
                _mediator.Object, _eventRepository.Object,
                _configuration.Object, _logger.Object);

            _eventRepository.Setup(x => x.GetEventFailureCount(It.IsAny<long>()))
                            .ReturnsAsync(_tryCount);

            _eventRepository.Setup(
               x => x.SetEventFailureCount(It.IsAny<long>(), It.IsAny<int>()))
               .Returns(Task.Delay(0));

            _eventRepository.Setup(
                x => x.StoreLastProcessedEventId(nameof(ApprenticeshipEventView), It.IsAny<long>()))
                .Returns(Task.Delay(0));

            _mediator.Setup(m => m.SendAsync(It.IsAny<CreateCommitmentApprenticeshipEntryCommand>()))
                .Throws<ApplicationException>();
        }

        [Test]
        public void ThenTheLastProcessedEventShouldBeIncreamentedIfMaxRetryCountHasNotBeenReached()
        {
            //Act
            Assert.ThrowsAsync<ApplicationException>(async ()=> await _handler.Handle(new ApprenticeshipEventView()));

            //Assert
            _eventRepository.Verify(
                x => x.SetEventFailureCount(It.IsAny<long>(), _tryCount + 1), Times.Once);

            _eventRepository.Verify(
                x => x.StoreLastProcessedEventId(nameof(ApprenticeshipEventView), It.IsAny<long>()), Times.Never);

        }

        [Test]
        public void ThenTheEventShouldBeIgnoredIfMaxRetryCountReached()
        {
            //Arrange
            _tryCount = _eventRetryCount + 1;

            _eventRepository.Setup(x => x.GetEventFailureCount(It.IsAny<long>()))
                           .ReturnsAsync(_tryCount);

            //Act
            Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(new ApprenticeshipEventView()));

            //Assert
            _eventRepository.Verify(
                x => x.StoreLastProcessedEventId(nameof(ApprenticeshipEventView), It.IsAny<long>()), Times.Once);
        }
    }
}
