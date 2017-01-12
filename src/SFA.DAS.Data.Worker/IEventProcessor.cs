using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker
{
    public interface IEventProcessor
    {
        Task ProcessEvents();
    }
}
