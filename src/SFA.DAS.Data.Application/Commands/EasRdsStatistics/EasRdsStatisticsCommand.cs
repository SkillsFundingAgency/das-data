using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Commands.EasRdsStatistics
{
    public class EasRdsStatisticsCommand : IAsyncRequest<EasRdsStatisticsCommandResponse>, IAsyncRequest<EasRdsStatisticsCommandHandler>
    {
        public EasStatisticsModel EasStatisticsModel { get; set; }
        public RdsStatisticsForEasModel RdsStatisticsForEasModel { get; set; }
    }
}
