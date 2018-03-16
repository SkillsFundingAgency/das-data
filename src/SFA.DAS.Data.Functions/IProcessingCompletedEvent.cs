using System;

namespace SFA.DAS.Data.Functions
{
    public interface IProcessingCompletedEvent
    {
        DateTime ProcessingCompletedAt { get; set; }
    }
}