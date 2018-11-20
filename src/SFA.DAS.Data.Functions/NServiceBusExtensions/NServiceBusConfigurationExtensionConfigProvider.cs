using System;
using Microsoft.Azure.WebJobs.Host.Config;
using SFA.DAS.Data.Functions.Extensions;

namespace SFA.DAS.Data.Functions.NServiceBusExtensions
{
    public class NServiceBusConfigurationExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            //Option: Add extension config provider here - this should be loaded because we already have another BindingAttribute for Inject.
            Console.WriteLine("Initializing NServiceBusConfigurationExtensionConfigProvider");

        
            //Workaround for redirecting Newtonsoft assembly
            //TODO: Use a separate extension config
            Startup.Startup.RedirectAssembly();
        
            //Consider doing this from the binding provider?
            //var loader = new NServiceBusConfigurationLoader();
            //loader.LoadNServiceBusConfiguration().Wait();
        }
    }
}
