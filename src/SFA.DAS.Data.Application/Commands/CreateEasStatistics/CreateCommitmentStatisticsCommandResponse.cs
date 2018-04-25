using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.Commands.EasRdsStatistics
{
    public class CreateEasStatisticsCommandResponse : ICommandResponse
    {
        public bool OperationSuccessful { get; set; }
    }
}
