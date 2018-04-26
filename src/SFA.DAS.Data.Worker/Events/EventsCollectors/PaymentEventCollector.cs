using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class PaymentEventCollector : PeriodEndEventsCollector<Payment>
    {
        private readonly IDataConfiguration _config;

        public PaymentEventCollector(IProviderEventService eventService, ILog logger, IDataConfiguration config)
            : base(eventService,logger)
        {
            _config = config;
        }

        protected override bool IsEnabled => _config.PaymentsEnabled;
    }
}
