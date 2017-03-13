using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventsCollectors
{
    public interface IEventsCollector<T> where T : IEventView
    {
        Task<ICollection<T>> GetEvents();
    }
}
