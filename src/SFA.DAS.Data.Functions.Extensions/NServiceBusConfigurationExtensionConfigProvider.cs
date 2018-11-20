using System;
using Microsoft.Azure.WebJobs.Host.Config;

namespace SFA.DAS.Data.Functions.Extensions
{
    public class NServiceBusConfigurationExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            Console.WriteLine("Initializing NServiceBusConfigurationExtensionConfigProvider");

            var loader = new NServiceBusConfigurationLoader();
            loader.LoadNServiceBusConfiguration().Wait();
        }
    }
}
