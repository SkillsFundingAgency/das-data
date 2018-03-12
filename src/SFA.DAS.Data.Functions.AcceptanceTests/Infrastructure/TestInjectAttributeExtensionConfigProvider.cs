using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using SFA.DAS.Data.Functions.Ioc;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure
{
    public class TestInjectAttributeExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly TestInjectAttributeBindingProvider _bindingProvider;
        public TestInjectAttributeExtensionConfigProvider()
        {
            _bindingProvider = new TestInjectAttributeBindingProvider();
        }
        public void Initialize(ExtensionConfigContext context)
        {
            context.Config.RegisterBindingExtensions(_bindingProvider);

            //context.AddBindingRule<InjectAttribute>().Bind(_bindingProvider);
        }
    }
}
