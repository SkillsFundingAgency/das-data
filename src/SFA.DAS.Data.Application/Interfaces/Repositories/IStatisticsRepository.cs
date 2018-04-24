using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IStatisticsRepository
    {
        Task<RdsStatisticsForEasModel> RetrieveEquivalentEasStatisticsFromRds();

        Task SaveEasStatistics(EasStatisticsModel easStatisticsModel,
            RdsStatisticsForEasModel rdsStatisticsForEasModel);
    }
}
