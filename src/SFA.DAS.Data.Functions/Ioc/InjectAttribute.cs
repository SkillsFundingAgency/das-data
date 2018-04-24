using System;
using Microsoft.Azure.WebJobs.Description;

namespace SFA.DAS.Data.Functions.Ioc
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    //[DefaultRegistryAttribute]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(string fullyQualifiedName)
        {
            
        }

        public InjectAttribute()
        {
            var x = this;
        }
       
    }
}
