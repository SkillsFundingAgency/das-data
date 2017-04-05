using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NLog;
using SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;
using ApprenticeshipEvent = SFA.DAS.Data.Domain.Models.ApprenticeshipEvent;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class ApprenticeshipEventHandler : EventApiHandler<ApprenticeshipEventView>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ApprenticeshipEventHandler(
            IMediator mediator, IMapper mapper, IEventRepository eventRepository, 
            IDataConfiguration configuration, ILog logger) 
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        protected override async Task ProcessEvent(ApprenticeshipEventView @event)
        {
            var commandEvent = _mapper.Map<ApprenticeshipEvent>(@event);

            await _mediator.SendAsync(new CreateCommitmentApprenticeshipEntryCommand
            {
                Event = commandEvent
            });
        }
    }
}
