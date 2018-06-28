using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using SFA.DAS.Data.Functions.Ioc;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure
{
    public class TestInjectAttributeBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parameterInfo = context.Parameter;
            var injectAttribute = parameterInfo.GetCustomAttribute<InjectAttribute>();
            if (injectAttribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            var objectResolver = new TestStructureMapObjectResolver();
            return Task.FromResult<IBinding>(new InjectAttributeBinding(parameterInfo, objectResolver));
        }
    }
}
