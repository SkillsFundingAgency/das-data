﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IEasStatisticsHandler
    {
        Task<EasStatisticsModel> Handle();
    }
}