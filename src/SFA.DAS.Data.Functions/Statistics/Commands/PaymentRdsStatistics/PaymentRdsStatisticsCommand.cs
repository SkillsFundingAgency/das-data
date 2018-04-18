using MediatR;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Functions.Statistics.Commands.PaymentRdsStatistics
{
    public class PaymentRdsStatisticsCommand : IAsyncRequest<PaymentRdsStatisticsCommandResponse>, IAsyncRequest<PaymentRdsStatisticsCommandHandler>, IStatisticsCommand<PaymentStatisticsModel, RdsStatisticsForPaymentsModel>
    {
        public PaymentStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForPaymentsModel RdsStatisticsModel { get; set; }
    }
}
