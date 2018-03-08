using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            context.AddBindingRule<InjectAttribute>().Bind(_bindingProvider);
        }
    }
}
