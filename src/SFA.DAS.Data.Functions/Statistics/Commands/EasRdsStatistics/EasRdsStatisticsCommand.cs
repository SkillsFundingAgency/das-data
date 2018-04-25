using MediatR;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Functions.Statistics.Commands.EasRdsStatistics
{
    public class EasRdsStatisticsCommand : IAsyncRequest<EasRdsStatisticsCommandResponse>, IAsyncRequest<EasRdsStatisticsCommandHandler>,
        IStatisticsCommand<EasStatisticsModel, RdsStatisticsForEasModel>
    {
        public EasStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForEasModel RdsStatisticsModel { get; set; }
    }
}
