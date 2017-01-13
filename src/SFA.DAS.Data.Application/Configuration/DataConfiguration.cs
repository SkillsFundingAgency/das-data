using Microsoft.Azure;

namespace SFA.DAS.Data.Application.Configuration
{
    public class DataConfiguration
    {
        public string DatabaseConnectionString => CloudConfigurationManager.GetSetting("DataConnectionString");

        public EventsApiClientConfiguration EventsApi => new EventsApiClientConfiguration();
    }
}
