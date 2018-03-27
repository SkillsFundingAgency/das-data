using System;
using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Functions
{
    public class CommitmentProcessingCompletedMessage : IProcessingCompletedMessage
    {
        public DateTime ProcessingCompletedAt { get; set; }
    }
}
