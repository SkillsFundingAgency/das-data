using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.Commands.CreatePaymentsStatistics
{
    public class CreatePaymentsStatisticsCommandResponse : ICommandResponse
    {
        public bool OperationSuccessful { get; set; }
    }
}
