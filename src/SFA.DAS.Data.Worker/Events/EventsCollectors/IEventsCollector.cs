using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public interface IEventsCollector<T>
    {
        Task<ICollection<T>> GetEvents();
    }
}
