using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;

namespace SFA.DAS.Data.Application.Commands.CommitmentRdsStatistics
{
    public class CreateCommitmentStatisticsCommand : IAsyncRequest<CreateCommitmentStatisticsCommandResponse>, IAsyncRequest<CreateCommitmentStatisticsCommandHandler>, IStatisticsCommand<CommitmentsExternalModel, CommitmentsRdsModel>
    {
        public CommitmentsExternalModel ExternalStatisticsModel { get; set; }
        public CommitmentsRdsModel RdsStatisticsModel { get; set; }
    }
}
