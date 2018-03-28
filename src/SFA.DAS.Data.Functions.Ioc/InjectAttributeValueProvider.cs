using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace SFA.DAS.Data.Functions.Ioc
{
    internal class InjectAttributeValueProvider : IValueProvider
    {
        private readonly ParameterInfo _parameterInfo;
        private readonly IObjectResolver _objectResolver;

        public InjectAttributeValueProvider(ParameterInfo parameterInfo, IObjectResolver objectResolver)
        {
            _parameterInfo = parameterInfo;
            _objectResolver = objectResolver;
        }

        public Task<object> GetValueAsync()
        {
            return Task.FromResult(_objectResolver.Resolve(Type));
        }

        public string ToInvokeString()
        {
            return Type.ToString();
        }

        public Type Type => _parameterInfo.ParameterType;
    }
}
