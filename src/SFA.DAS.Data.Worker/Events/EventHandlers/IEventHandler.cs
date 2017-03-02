using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events.EventHandlers
{
    public interface IEventHandler<in T> where T : IEventView
    {
        void Handle(T @event);
    }
}
