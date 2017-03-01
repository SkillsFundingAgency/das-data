using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker
{
    public interface IEventsProcessor
    {
        Task ProcessEvents();
    }
}
