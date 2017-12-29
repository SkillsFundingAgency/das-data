using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateEmploymentCheck;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EmploymentCheck.Events;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class EmploymentCheckCompleteEventHandler : EventApiHandler<GenericEvent<EmploymentCheckCompleteEvent>>
    {
        private readonly IMediator _mediator;

        public EmploymentCheckCompleteEventHandler(
            IMediator mediator,
            IEventRepository eventRepository,
            IDataConfiguration configuration,
            ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(GenericEvent<EmploymentCheckCompleteEvent> @event)
        {
            await _mediator.PublishAsync(new CreateEmploymentCheckCommand { Event = @event.Payload });
        }
    }
}
