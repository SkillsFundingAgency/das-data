using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreateDataLock;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class DataLockEventHandler : EventHandler<DataLockEvent>
    {
        private readonly IMediator _mediator;

        public static string EventTypeName => typeof(DataLockEvent).Name;

        public DataLockEventHandler(IMediator mediator, IEventRepository eventRepository, IDataConfiguration configuration, ILog logger)
            : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        public override async Task Handle(DataLockEvent @event)
        {
            await Handle(@event, EventTypeName, @event.Id);
        }

        protected override async Task ProcessEvent(DataLockEvent @event)
        {
            await _mediator.PublishAsync(new CreateDataLockCommand { Event = @event });
        }
    }
}
