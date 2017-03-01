using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public interface IEventsCollector<T>
    {
        Task<ICollection<T>> GetEvents();
    }
}
