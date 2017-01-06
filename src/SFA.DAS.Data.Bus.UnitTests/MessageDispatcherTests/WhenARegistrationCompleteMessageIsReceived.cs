using System.Threading.Tasks;
using MediatR;
using Microsoft.ServiceBus.Messaging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRegistration;

namespace SFA.DAS.Data.Bus.UnitTests.MessageDispatcherTests
{
    [TestFixture]
    public class WhenARegistrationCompleteMessageIsReceived
    {
        private MessageDispatcher _messageDispatcher;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _messageDispatcher = new MessageDispatcher(_mediator.Object);
        }

        [Test]
        public async Task ThenTheCreateRegistrationCommandIsCalled()
        {
            var dasAccountId = "375";
            var message = new BrokeredMessage(dasAccountId);

            await _messageDispatcher.Dispatch(message);

            _mediator.Verify(m => m.SendAsync(It.Is<CreateRegistrationCommand>(c => c.DasAccountId == dasAccountId)));
        }
    }
}
