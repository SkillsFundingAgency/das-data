﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IProcessingCompletedMessage
    {
        DateTime ProcessingCompletedAt { get; set; }
    }
}
