using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.Commands.PaymentRdsStatistics
{
    public class CreatePaymentsStatisticsCommandResponse : ICommandResponse
    {
        public bool OperationSuccessful { get; set; }
    }
}
