using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Commands.VerifyRelationship;
using SFA.DAS.Data.Worker.MessageProcessors;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.MessageProcessors.RelationshipVerifiedMessageProcessorTests
{
    [TestFixture]
    public class WhenARelationshipVerifiedMessageIsProcesed
    {
        private Mock<IMessageSubscriberFactory> _subscriberFactory;
        private Mock<IMessageSubscriber<RelationshipVerified>> _subscriber;
        private RelationshipVerified _messageContent;
        private Mock<IMessage<RelationshipVerified>> _mockMessage;
        private Mock<IMediator> _mediator;
        private CancellationTokenSource _tokenSource;
        private RelationshipVerifiedMessageProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _subscriberFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<RelationshipVerified>>();

            _messageContent = new RelationshipVerified
            {
                ProviderId = 1,
                EmployerAccountId = 2,
                LegalEntityId = "3",
                Verified = true
            };

            _mockMessage = new Mock<IMessage<RelationshipVerified>>();
            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new RelationshipVerifiedMessageProcessor(_subscriberFactory.Object, Mock.Of<ILog>(), _mediator.Object);

            _subscriberFactory.Setup(x => x.GetSubscriber<RelationshipVerified>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync()).ReturnsAsync(() => _mockMessage.Object)
                .Callback(() => { _tokenSource.Cancel(); });
        }

        [Test]
        public async Task ThenTheRelationshipVerifiedMessageIsProcessed()
        {
            // Act
            await _processor.RunAsync(_tokenSource);

            // Assert
            _mediator.Verify(
                x => x.PublishAsync(It.Is<VerifyRelationshipCommand>(c =>
                    c.ProviderId == _messageContent.ProviderId &&
                    c.EmployerAccountId == _messageContent.EmployerAccountId &&
                    c.LegalEntityId == _messageContent.LegalEntityId && c.Verified == _messageContent.Verified)),
                Times.Once);
        }
    }
}
