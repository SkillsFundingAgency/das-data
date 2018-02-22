using System.Collections.Generic;
using SFA.DAS.Messaging.AzureServiceBus.StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.Configuration
{
    public class DataMessagePublishConfiguration : ITopicMessagePublisherConfiguration
    {
        public string MessageServiceBusConnectionString { get; set; }
        public Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
