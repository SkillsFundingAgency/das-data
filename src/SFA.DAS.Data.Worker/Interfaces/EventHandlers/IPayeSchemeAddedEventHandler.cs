using SFA.DAS.Events.Api.Types;
using SFA.DAS.Events.Dispatcher;

namespace SFA.DAS.Data.Worker.Interfaces.EventHandlers
{
    public interface IPayeSchemeAddedEventHandler : IEventHandler<AccountEventView>
    {
    }
}
