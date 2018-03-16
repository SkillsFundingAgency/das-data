using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Functions
{
    public class CommitmentProcessingCompletedEvent : IProcessingCompletedEvent
    {
        public DateTime ProcessingCompletedAt { get; set; }
    }
}
