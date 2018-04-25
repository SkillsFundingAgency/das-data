using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;

namespace SFA.DAS.Data.Application.Commands.PaymentRdsStatistics
{
    public class PaymentRdsStatisticsCommand : IAsyncRequest<PaymentRdsStatisticsCommandResponse>, IAsyncRequest<PaymentRdsStatisticsCommandHandler>, IStatisticsCommand<PaymentStatisticsModel, RdsStatisticsForPaymentsModel>
    {
        public PaymentStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForPaymentsModel RdsStatisticsModel { get; set; }
    }
}
