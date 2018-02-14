using System;

namespace SFA.DAS.Data.Infrastructure.Attributes
{
    public class ServiceBusConnectionStringAttribute : Attribute
    {
        public string Name { get; set; }

        public ServiceBusConnectionStringAttribute(string name)
        {
            Name = name;
        }
    }
}
