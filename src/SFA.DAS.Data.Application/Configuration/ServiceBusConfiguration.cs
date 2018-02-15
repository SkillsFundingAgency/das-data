using System.Collections.Generic;

namespace SFA.DAS.Data.Application.Configuration
{
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public string MessageServiceBusConnectionString { get; set; }
        public Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
