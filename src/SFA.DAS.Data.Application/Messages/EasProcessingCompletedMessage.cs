using System;
using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Application.Messages
{
    public class EasProcessingCompletedMessage : IProcessingCompletedMessage
    {
        public DateTime ProcessingCompletedAt { get; set; }
    }
}
