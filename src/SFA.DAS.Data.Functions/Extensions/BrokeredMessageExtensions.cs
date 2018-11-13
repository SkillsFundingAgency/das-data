using System.IO;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace SFA.DAS.Data.Functions.Extensions
{
    public static class BrokeredMessageExtensions
    {
        public static T DeserializeJsonMessage<T>(this BrokeredMessage message)
        {
            using (var stream = message.GetBody<Stream>())
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<T>(jsonReader);
                return result;
            }
        }
    }
}
