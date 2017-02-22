using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.EventHandlers
{
    public class ApprenticeshipEventHandler 
    {
        private readonly IMediator _mediator;

        public ApprenticeshipEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ApprenticeshipEventView @event)
        {
           throw new NotImplementedException();
        }
    }
}
