using MediatR;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.Data.Domain.Messages
{
    public abstract class EventNotification<EventType> : INotification
        where EventType : Event
    {
        public EventNotification(EventType eventPayload)
        {
            EventPayload = eventPayload;
        }

        public EventType EventPayload { get; }
    }
}