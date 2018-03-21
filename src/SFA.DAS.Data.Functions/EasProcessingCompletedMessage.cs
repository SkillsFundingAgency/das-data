using System;
using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Functions
{
    public class EasProcessingCompletedMessage : IProcessingCompletedMessage
    {
        public DateTime ProcessingCompletedAt { get; set; }
    }
}
