using System;
using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Application.Commands.Statistics
{
    public class EasProcessingCompletedMessage : IProcessingCompletedMessage
    {
        public DateTime ProcessingCompletedAt { get; set; }
    }
}
