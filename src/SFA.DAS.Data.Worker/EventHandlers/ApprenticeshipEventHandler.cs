using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Worker.Interfaces.EventHandlers;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.EventHandlers
{
    public class ApprenticeshipEventHandler : IApprenticeshipEventHandler
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ApprenticeshipEventHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Handle(ApprenticeshipEventView @event)
        {
            var commandEvent = _mapper.Map<CommitmentsApprenticeshipEvent>(@event);

            await _mediator.SendAsync(new CreateCommitmentApprenticeshipEntryCommand
            {
                Event = commandEvent
            });
        }
    }
}
