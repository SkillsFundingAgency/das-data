using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Commands.VerifyRelationship;
using SFA.DAS.Data.Infrastructure.Attributes;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.MessageProcessors
{
    [ServiceBusConnectionString("Commitments")]
    [TopicSubscription("RDS_RelationshipVerified")]
    public class RelationshipVerifiedMessageProcessor : MessageProcessor<RelationshipVerified>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public RelationshipVerifiedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) : base(subscriberFactory, log)
        {
            _logger = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(RelationshipVerified messageContent)
        {
            _logger.Debug($"Relationship verified.  Updating relationship with Verified={messageContent.Verified}, for ProviderId={messageContent.ProviderId}, EmployerAccountId={messageContent.EmployerAccountId}, LegalEntityId={messageContent.LegalEntityId}");

            await _mediator.PublishAsync(new VerifyRelationshipCommand
            {
                ProviderId = messageContent.ProviderId,
                EmployerAccountId = messageContent.EmployerAccountId,
                LegalEntityId = messageContent.LegalEntityId,
                Verified = messageContent.Verified
            });
        }
    }
}
