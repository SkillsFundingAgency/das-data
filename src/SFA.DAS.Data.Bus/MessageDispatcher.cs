using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Data.Application.Commands.CreateRegistration;

namespace SFA.DAS.Data.Bus
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IMediator _mediator;

        public MessageDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Dispatch(BrokeredMessage receivedMessage)
        {
            // This ridiculously simple implementation will only work for a single message type and needs to be replaced by something much better.
            // Only option for this will be to use something like MassTransit or NServiceBus on top of Azure Service Bus as it'll handle message dispatching, and a whole lot more.
            var dasAccountId = receivedMessage.GetBody<string>();
            await _mediator.SendAsync(new CreateRegistrationCommand {DasAccountId = dasAccountId});
        }
    }
}
