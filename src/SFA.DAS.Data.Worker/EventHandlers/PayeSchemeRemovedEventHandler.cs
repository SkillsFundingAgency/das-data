﻿using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.RemovePayeScheme;
using SFA.DAS.Data.Worker.Interfaces.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.EventHandlers
{
    public class PayeSchemeRemovedEventHandler : IPayeSchemeRemovedEventHandler
    {
        private readonly IMediator _mediator;

        public PayeSchemeRemovedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(AccountEventView @event)
        {
            await _mediator.PublishAsync(new RemovePayeSchemeCommand { PayeSchemeHref = @event.ResourceUri });
        }
    }
}