using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;

namespace SFA.DAS.Data.Application.Commands.CommitmentRdsStatistics
{
    public class CommitmentRdsStatisticsCommand : IAsyncRequest<CommitmentRdsStatisticsCommandResponse>, IAsyncRequest<CommitmentRdsStatisticsCommandHandler>, IStatisticsCommand<CommitmentsExternalModel, CommitmentsRdsModel>
    {
        public CommitmentsExternalModel ExternalStatisticsModel { get; set; }
        public CommitmentsRdsModel RdsStatisticsModel { get; set; }
    }
}
