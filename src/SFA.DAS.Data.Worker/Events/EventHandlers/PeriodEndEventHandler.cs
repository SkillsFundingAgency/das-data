using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class PeriodEndEventHandler : EventHandler<PeriodEnd>
    {
        private readonly IMediator _mediator;
        
        public PeriodEndEventHandler(IMediator mediator, IEventRepository eventRepository, IDataConfiguration configuration, ILog logger) : base(eventRepository, configuration, logger)
        {
            _mediator = mediator;
        }

        protected override async Task ProcessEvent(PeriodEnd @event)
        {
            Task.WaitAll(
                _mediator.PublishAsync(new CreatePaymentsForPeriodEndCommand { PeriodEndId = @event.Id }),
                _mediator.PublishAsync(new CreatePaymentsForPeriodEndCommand { PeriodEndId = @event.Id })
            );
        }

        public override async Task Handle(PeriodEnd @event)
        {
            await Handle(@event, typeof(PeriodEnd).Name, @event.Id);
        }
    }
}
