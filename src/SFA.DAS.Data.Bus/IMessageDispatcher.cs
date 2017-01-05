using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SFA.DAS.Data.Bus
{
    public interface IMessageDispatcher
    {
        Task Dispatch(BrokeredMessage receivedMessage);
    }
}
