using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker
{
    public interface IEventsWatcher
    {
        Task ProcessEvents();
    }
}
