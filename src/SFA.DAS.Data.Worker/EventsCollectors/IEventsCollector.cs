using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker.EventsCollectors
{
    public interface IEventsCollector<T>
    {
        Task<ICollection<T>> GetEvents();
    }
}
