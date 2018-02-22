using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SFA.DAS.Data.AcceptanceTests.DependencyResolution
{
    public interface IAzureTopicMessageBus
    {
        Task PublishAsync(object message);

        Task PublishAsync(object message, string messageGroupName);

        //Task<BrokeredMessage> PeekAsync(object message);
    }
}
