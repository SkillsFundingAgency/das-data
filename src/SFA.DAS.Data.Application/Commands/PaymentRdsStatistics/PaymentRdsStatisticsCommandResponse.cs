using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.Commands.PaymentRdsStatistics
{
    public class PaymentRdsStatisticsCommandResponse : ICommandResponse
    {
        public bool OperationSuccessful { get; set; }
    }
}
