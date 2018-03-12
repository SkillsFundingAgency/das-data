using MediatR;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Functions.Commands.EasRdsStatistics
{
    public class EasRdsStatisticsCommand : IAsyncRequest<EasRdsStatisticsCommandResponse>, IAsyncRequest<EasRdsStatisticsCommandHandler>
    {
        public EasStatisticsModel EasStatisticsModel { get; set; }
        public RdsStatisticsForEasModel RdsStatisticsForEasModel { get; set; }
    }
}
