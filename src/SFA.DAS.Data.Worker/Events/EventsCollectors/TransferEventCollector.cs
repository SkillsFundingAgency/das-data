using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public class TransferEventCollector : PeriodEndEventsCollector<AccountTransfer>
    {
        private readonly IDataConfiguration _config;

        public TransferEventCollector(IProviderEventService eventService, ILog logger, IDataConfiguration config)
            : base(eventService,logger)
        {
            _config = config;
        }

        protected override bool IsEnabled => _config.TransfersEnabled;
    }
}
