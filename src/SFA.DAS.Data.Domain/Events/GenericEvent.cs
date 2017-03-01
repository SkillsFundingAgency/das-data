using System;

namespace SFA.DAS.Data.Domain.Events
{
    public class GenericEvent
    {
        public long Id { get; set; }
        public string Event { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
