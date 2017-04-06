using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IProviderEventService
    {
        Task<ICollection<PeriodEnd>> GetUnprocessedPeriodEnds();

        Task<PageOfResults<Payment>> GetPayments(string periodId, int pageNumber);
    }
}
