using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.Commands.CommitmentRdsStatistics
{
    public class CommitmentRdsStatisticsCommandResponse : ICommandResponse
    {
        public bool OperationSuccessful { get; set; }
    }
}
