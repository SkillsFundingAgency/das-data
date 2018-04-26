using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Provider.Events.Api.Client;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class ProviderEventsService : IProviderEventService
    {
        private readonly IPaymentsEventsApiClient _eventsApi;
        private readonly IEventRepository _eventRepository;

        public ProviderEventsService(IPaymentsEventsApiClient eventsApi, IEventRepository eventRepository)
        {
            _eventsApi = eventsApi;
            _eventRepository = eventRepository;
        }

        public async Task<ICollection<PeriodEnd>> GetUnprocessedPeriodEnds<T>()
        {
            var lastProcessedPeriodId = await _eventRepository.GetLastProcessedEventId<string>(GetFeedName<T>());
            var periodEnds = await _eventsApi.GetPeriodEnds();

            if (!HaveAnyPeriodsBeenProcessedPreviously(lastProcessedPeriodId))
                return periodEnds;

            return GetUnprocessedPeriods(periodEnds, lastProcessedPeriodId);
        }

        private static string GetFeedName<T>()
        {
            return string.Concat(typeof(PeriodEnd).Name, "-", typeof(T).Name);
        }

        public async Task<PageOfResults<Payment>> GetPayments(string periodId, int pageNumber)
        {
            return await _eventsApi.GetPayments(periodId, null, pageNumber);
        }

        public async Task<PageOfResults<AccountTransfer>> GetTransfers(string periodId, int pageNumber)
        {
            return await _eventsApi.GetTransfers(periodId, page: pageNumber);
        }

        private static ICollection<PeriodEnd> GetUnprocessedPeriods(PeriodEnd[] periodEnds, string lastProcessedPeriodId)
        {
            return periodEnds.SkipWhile(x => x.Id != lastProcessedPeriodId).Skip(1).ToList();
        }

        private static bool HaveAnyPeriodsBeenProcessedPreviously(string lastProcessedPeriodId)
        {
            return !string.IsNullOrEmpty(lastProcessedPeriodId);
        }
    }
}
