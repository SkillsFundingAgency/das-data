using MediatR;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateDataLock
{
    public class CreateDataLockCommand : IAsyncNotification
    {
        public DataLockEvent Event { get; set; }
    }
}
