using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Events;
using SFA.DAS.Events.Api.Types;
using GenericEvent = SFA.DAS.Events.Api.Types.GenericEvent;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IEventService
    {
        Task<ICollection<GenericEvent>> GetGenericEvents(string eventType);

        Task<ICollection<ApprenticeshipEvent>> GetApprenticeshipEvents();
    }
}
