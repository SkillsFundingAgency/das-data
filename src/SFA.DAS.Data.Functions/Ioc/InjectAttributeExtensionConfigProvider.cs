using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;

namespace SFA.DAS.Data.Functions.Ioc
{
    public class InjectAttributeExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly InjectAttributeBindingProvider _bindingProvider;
        public InjectAttributeExtensionConfigProvider()
        {
            _bindingProvider = new InjectAttributeBindingProvider();
        }
        public void Initialize(ExtensionConfigContext context)
        {
            context.Config.RegisterBindingExtensions(_bindingProvider);
        }
    }
}
