using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Data.Worker.Events.EventHandlers;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.Events.EventHandlerTests.PeriodEndEventHandlerTests
{
    public class WhenAPeriodEndEventIsHandled
    {
        private Mock<IMediator> _mediator;
        private PaymentEventHandler _handler;
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


            _handler = new PaymentEventHandler(_mediator.Object, _eventRepository.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheCreateEventCommandShouldBeSent()
        {
            //Arrange
            var periodEndEvent = new PeriodEndEvent<Payment> {PeriodEnd = new PeriodEnd {Id = "ABC123"}};

            //Act
            await _handler.Handle(periodEndEvent);

            //Assert
            _mediator.Verify(x => x.PublishAsync(It.Is<CreatePaymentsForPeriodEndCommand>(c => c.PeriodEndId == periodEndEvent.PeriodEnd.Id)), Times.Once);
        }
    }
}
