using MediatR;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Functions.Statistics.Commands.CommitmentRdsStatistics
{
    public class CommitmentRdsStatisticsCommand : IAsyncRequest<CommitmentRdsStatisticsCommandResponse>, IAsyncRequest<CommitmentRdsStatisticsCommandHandler>, IStatisticsCommand<CommitmentsStatisticsModel, RdsStatisticsForCommitmentsModel>
    {
        public CommitmentsStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForCommitmentsModel RdsStatisticsModel { get; set; }
    }
}
