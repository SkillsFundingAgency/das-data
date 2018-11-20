using System;

namespace SFA.DAS.Data.Functions.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NServiceBusConfigurationAttribute : Attribute
    {
        public string Connection { get; private set; }

        public string Queue { get; private set; }

        public string Subscription { get; private set; }

        public Type MessageType { get; private set; }

        public NServiceBusConfigurationAttribute(string subscription, Type messageType)
            : this(null, subscription, messageType, null)
        {
        }

        public NServiceBusConfigurationAttribute(string queue, string subscription, Type messageType)
            : this(queue, subscription, messageType, null)
        {
        }

        public NServiceBusConfigurationAttribute(string queue, string subscription, Type messageType, string connection)
        {
            Connection = connection;
            Queue = queue;
            Subscription = subscription;
            MessageType = messageType;
        }

    }
}
