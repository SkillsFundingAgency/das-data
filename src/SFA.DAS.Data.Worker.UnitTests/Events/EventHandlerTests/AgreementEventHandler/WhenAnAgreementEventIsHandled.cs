using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRoatpProvider;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.AgreementEventHandler
{
    public class WhenAnAgreementEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private Worker.Events.EventHandlers.AgreementEventHandler _handler;
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

            _handler = new Worker.Events.EventHandlers.AgreementEventHandler(_mediator.Object, _eventRepository.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheCreateProviderCommandShouldBeSent()
        {
            //Arrange
            var agrrementEventView = new AgreementEventView
            {
                Id = 1,
                ContractType = "ProviderAgreement",
                CreatedOn = DateTime.Now.AddDays(-1),
                Event = "INITIATED",
                ProviderId = "12345678"
            };

            //Act
            await _handler.Handle(agrrementEventView);

            //Assert
            _mediator.Verify(x => x.PublishAsync(It.Is<CreateProviderCommand>(c => c.Event.ProviderId == agrrementEventView.ProviderId)), Times.Once);
        }
    }
}
