using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public abstract class PeriodEndEventsCollector<T> : IEventsCollector<PeriodEndEvent<T>>
    {
        protected IProviderEventService EventService { get; }
        protected ILog Logger {get;}

        public PeriodEndEventsCollector(IProviderEventService eventService, ILog logger)
        {
            EventService = eventService;
            Logger = logger;
        }

        public async Task<ICollection<PeriodEndEvent<T>>> GetEvents()
        {
            Logger.Info("Getting unprocessed period ends");

            //if (!_config.PaymentsEnabled && !_config.TransfersEnabled)
            if (!IsEnabled)
                return new List<PeriodEndEvent<T>>();

            var apiEvents = await EventService.GetUnprocessedPeriodEnds<T>();

            if (apiEvents == null)
                return new List<PeriodEndEvent<T>>();

            Logger.Info($"{apiEvents?.Count} periods retrieved from provider events service");

            return apiEvents.Select(p => new PeriodEndEvent<T> {PeriodEnd = p}).ToList();
        }

        protected abstract bool IsEnabled {get;}
    }
}
