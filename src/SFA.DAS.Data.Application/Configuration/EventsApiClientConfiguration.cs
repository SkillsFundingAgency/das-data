using Microsoft.Azure;
using SFA.DAS.Events.Api.Client.Configuration;

namespace SFA.DAS.Data.Application.Configuration
{
    public class EventsApiClientConfiguration : IEventsApiClientConfiguration
    {
        public string BaseUrl
        {
            get { return CloudConfigurationManager.GetSetting("EventsApiBaseUrl"); }
            set { }
        }

        public string ClientToken
        {
            get { return CloudConfigurationManager.GetSetting("EventsApiClientToken"); }
            set { }
        }
    }
}
