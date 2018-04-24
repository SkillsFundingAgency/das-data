using System;
using Microsoft.Azure.WebJobs.Description;

namespace SFA.DAS.Data.Functions.Ioc
{

    public class DefaultRegistryAttribute : Attribute
    {
        public DefaultRegistry defaultRegistry;
        public DefaultRegistryAttribute()
        {

        }
        public DefaultRegistryAttribute(string fullyQualifiedClassName)
        {
            
        }
    }
}
