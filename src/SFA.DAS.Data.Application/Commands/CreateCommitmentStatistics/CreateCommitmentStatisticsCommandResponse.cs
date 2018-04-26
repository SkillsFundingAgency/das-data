using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.Commands.CreateCommitmentStatistics
{
    public class CreateCommitmentStatisticsCommandResponse : ICommandResponse
    {
        public bool OperationSuccessful { get; set; }
    }
}
