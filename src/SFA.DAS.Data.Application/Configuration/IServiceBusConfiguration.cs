using System.Collections.Generic;
using SFA.DAS.Configuration;

namespace SFA.DAS.Data.Application.Configuration
{
    public interface IServiceBusConfiguration : IConfiguration
    {
        Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
