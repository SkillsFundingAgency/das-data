using SFA.DAS.Events.Api.Types;
using SFA.DAS.Events.Dispatcher;

namespace SFA.DAS.Data.Worker.EventHandlers
{
    public interface IAccountCreatedEventHandler : IEventHandler<AccountEventView>
    {
    }
}
