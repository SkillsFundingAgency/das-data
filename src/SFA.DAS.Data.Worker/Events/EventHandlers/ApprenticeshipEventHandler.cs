using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class ApprenticeshipEventHandler : EventApiHandler<ApprenticeshipEventView>
    {
        private readonly IMediator _mediator;

        public ApprenticeshipEventHandler(
            IMediator mediator, IEventRepository eventRepository, 
            IDataConfiguration configuration, ILog logger) 
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(ApprenticeshipEventView @event)
        {
            await _mediator.SendAsync(new CreateCommitmentApprenticeshipEntryCommand
            {
                Event = @event
            });
        }
    }
}
