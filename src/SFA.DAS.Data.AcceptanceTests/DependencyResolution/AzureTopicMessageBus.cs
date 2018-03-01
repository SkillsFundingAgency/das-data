using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Messaging.Helpers;

namespace SFA.DAS.Data.AcceptanceTests.DependencyResolution
{
    public class AzureTopicMessageBus : IAzureTopicMessageBus
    {
        private readonly string _commitmentsServiceBusConnectionString;

        public AzureTopicMessageBus(string commitmentsServiceBus)
        {
            _commitmentsServiceBusConnectionString = commitmentsServiceBus;
        }

        public async Task PublishAsync(object message)
        {
            var messageGroupName = MessageGroupHelper.GetMessageGroupName(message);

            await PublishAsync(message, messageGroupName);
        }

        public async Task PublishAsync(object message, string messageGroupName)
        {
            TopicClient client = null;

            try
            {
                client = TopicClient.CreateFromConnectionString(_commitmentsServiceBusConnectionString, messageGroupName);
                await client.SendAsync(new BrokeredMessage(message)); 
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (client != null && !client.IsClosed)
                {
                    await client.CloseAsync();
                }
            }
        }
    }
}
