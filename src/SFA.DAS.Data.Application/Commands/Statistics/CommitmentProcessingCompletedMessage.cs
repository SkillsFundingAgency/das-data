using System;
using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Application.Commands.Statistics
{
    public class CommitmentProcessingCompletedMessage : IProcessingCompletedMessage
    {
        public DateTime ProcessingCompletedAt { get; set; }
    }
}
