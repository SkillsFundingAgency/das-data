using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public interface IEventHandler<in T> where T : IEventView
    {
        Task Handle(T @event);
    }
}
