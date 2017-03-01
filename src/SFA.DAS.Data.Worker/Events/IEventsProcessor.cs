using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker.Events
{
    public interface IEventsProcessor
    {
        Task ProcessEvents();
    }
}
