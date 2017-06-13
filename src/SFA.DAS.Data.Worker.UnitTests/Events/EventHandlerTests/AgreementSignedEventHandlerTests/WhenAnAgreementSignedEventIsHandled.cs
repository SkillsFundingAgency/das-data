using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateEmployerAgreement;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.EAS.Account.Api.Types.Events.Agreement;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.AgreementSignedEventHandlerTests
{
    public class WhenAnAgreementSignedEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private AgreementSignedEventHandler _handler;
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


            _handler = new AgreementSignedEventHandler(_mediator.Object, _eventRepository.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheCreateEmployerAgreementCommandShouldBeSent()
        {
            //Arrange
            var levyDeclaration = new GenericEvent<AgreementSignedEvent>
            {
                Payload = new AgreementSignedEvent { ResourceUrl = "AgreementUri" }
            };

            //Act
            await _handler.Handle(levyDeclaration);

            //Assert
            _mediator.Verify(x => x.PublishAsync(It.Is<CreateEmployerAgreementCommand>(c => c.AgreementHref == levyDeclaration.Payload.ResourceUrl)), Times.Once);
        }
    }
}
