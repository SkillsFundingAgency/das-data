using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;

namespace SFA.DAS.Data.Application.Commands.PaymentRdsStatistics
{
    public class CreatePaymentsStatisticsCommand : IAsyncRequest<CreatePaymentsStatisticsCommandResponse>, IAsyncRequest<CreatePaymentsStatisticsCommandHandler>, IStatisticsCommand<PaymentExternalModel, PaymentsRdsModel>
    {
        public PaymentExternalModel ExternalStatisticsModel { get; set; }
        public PaymentsRdsModel RdsStatisticsModel { get; set; }
    }
}
