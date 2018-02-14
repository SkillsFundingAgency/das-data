using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Commands.CreateRelationship;
using SFA.DAS.Data.Infrastructure.Attributes;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.MessageProcessors
{
    [ServiceBusConnectionString("Commitments")]
    [TopicSubscription("RDS_RelationshipCreated")]
    public class RelationshipCreatedMessageProcessor : MessageProcessor<RelationshipCreated>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public RelationshipCreatedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) : base(subscriberFactory, log)
        {
            _logger = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(RelationshipCreated message)
        {
            _logger.Debug($"Relationship created.  Saving new relationship for ProviderId={message.Relationship.ProviderId}, EmployerAccountId={message.Relationship.EmployerAccountId}, LegalEntityId={message.Relationship.LegalEntityId}");

            await _mediator.PublishAsync(new CreateRelationshipCommand {Relationship = message.Relationship});
        }
    }
}
