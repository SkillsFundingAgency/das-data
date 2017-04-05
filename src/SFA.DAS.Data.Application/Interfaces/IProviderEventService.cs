using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IProviderEventService
    {
        Task<ICollection<PeriodEnd>> GetUnprocessedPeriodEnds();
    }
}
