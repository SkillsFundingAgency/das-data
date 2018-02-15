using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Commands.CreateRelationship;
using SFA.DAS.Data.Worker.MessageProcessors;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.MessageProcessors.RelationshipCreatedMessageProcessorTests
{
    [TestFixture]
    public class WhenARelationshipCreatedMessageIsProcessed
    {
        private Mock<IMessageSubscriberFactory> _subscriberFactory;
        private Mock<IMessageSubscriber<RelationshipCreated>> _subscriber;
        private RelationshipCreated _messageContent;
        private Mock<IMessage<RelationshipCreated>> _mockMessage;
        private Mock<IMediator> _mediator;
        private CancellationTokenSource _tokenSource;
        private RelationshipCreatedMessageProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _subscriberFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<RelationshipCreated>>();

            _messageContent = new RelationshipCreated {Relationship = new Relationship()};

            _mockMessage = new Mock<IMessage<RelationshipCreated>>();
            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new RelationshipCreatedMessageProcessor(_subscriberFactory.Object, Mock.Of<ILog>(), _mediator.Object);

            _subscriberFactory.Setup(x => x.GetSubscriber<RelationshipCreated>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync()).ReturnsAsync(() => _mockMessage.Object)
                .Callback(() => { _tokenSource.Cancel(); });
        }

        [Test]
        public async Task ThenTheRelationshipCreatedMessageIsProcessed()
        {
            // Act
            await _processor.RunAsync(_tokenSource);

            // Assert
            _mediator.Verify(x=>x.PublishAsync(It.Is<CreateRelationshipCommand>(c=>c.Relationship == _messageContent.Relationship)), Times.Once() );
        }
    }
}
