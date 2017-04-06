using System.Threading.Tasks;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public interface IEventHandler<in T>
    {
        Task Handle(T @event);
    }
}
