using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using System.Collections.Generic;
using StructureMap;

namespace SFA.DAS.Data.Functions.Ioc
{
    public class InjectAttributeBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //var type = Type.GetType("context");

            var customAttributes = context.Parameter.CustomAttributes;


            #region stackoverflow
            //MemberInfo member = type.GetMember(string.Empty)[0];
            //var customAttributes = member.GetCustomAttributesData();
            Type type = null;
            foreach (var data in customAttributes)
            {
                foreach (var arg in data.ConstructorArguments)
                {
                    // The type and value of the constructor arguments,
                    // e.g. "System.String a supplied value"
                    Console.WriteLine(arg.ArgumentType + " " + arg.Value);
                    if (arg.Value.ToString() == "SFA.DAS.Data.Functions.Ioc.DefaultRegistry")
                        type = Type.GetType(arg.Value.ToString());
                }
            } 
            #endregion

            
            var myObject = Activator.CreateInstance(type);

            var parameterInfo = context.Parameter;
            var injectAttribute = parameterInfo.GetCustomAttribute<InjectAttribute>();
            if (injectAttribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            var objectResolver = new StructureMapObjectResolver((Registry) myObject);
            return Task.FromResult<IBinding>(new InjectAttributeBinding(parameterInfo, objectResolver));
        }
    }
}
