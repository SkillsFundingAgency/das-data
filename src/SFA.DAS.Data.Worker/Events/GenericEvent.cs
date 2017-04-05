using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.Events
{
    public class GenericEvent<T> : IEventView
    {
        public long Id { get; set; }
        public T Payload { get; set; }
        public string Type { get; set; }
    }
}
