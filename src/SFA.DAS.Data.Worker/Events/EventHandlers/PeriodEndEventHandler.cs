using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public abstract class PeriodEndEventHandler<T> : EventHandler<PeriodEndEvent<T>>
    {
        protected IMediator Mediator { get; }

        protected PeriodEndEventHandler(IMediator mediator, IEventRepository eventRepository, IDataConfiguration configuration, ILog logger) : base(eventRepository, configuration, logger)
        {
            Mediator = mediator;
        }

        public override async Task Handle(PeriodEndEvent<T> @event)
        {
            await Handle(@event, EventTypeName, @event.PeriodEnd.Id);
        }

        public static string EventTypeName => string.Concat(typeof(PeriodEnd).Name, "-", typeof(T).Name);
    }
}
