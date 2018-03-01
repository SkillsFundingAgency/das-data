using SFA.DAS.Data.AcceptanceTests.Configuration;
using SFA.DAS.Data.Infrastructure.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Data.AcceptanceTests.DependencyResolution
{
    public class TestRegistry : Registry
    {
        public TestRegistry()
        {
            var messagePublisher = ConfigurationHelper.GetConfiguration<DataMessagePublishConfiguration>("SFA.DAS.Data");
            var serviceBusConnectionString = messagePublisher.MessageServiceBusConnectionStringLookup;
            For<IAzureTopicMessageBus>().Use(new AzureTopicMessageBus(serviceBusConnectionString["Commitments"]));
        }
    }
}
