using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public class PaymentEventHandler : PeriodEndEventHandler<Payment>
    {
        public PaymentEventHandler(IMediator mediator, IEventRepository eventRepository, IDataConfiguration configuration, ILog logger) 
            : base(mediator, eventRepository, configuration, logger)
        {
        }

        protected override async Task ProcessEvent(PeriodEndEvent<Payment> @event)
        {
            await Mediator.PublishAsync(new CreatePaymentsForPeriodEndCommand {PeriodEndId = @event.PeriodEnd.Id});
        }
    }
}
