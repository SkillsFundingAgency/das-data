using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands.CommitmentRdsStatistics;

namespace SFA.DAS.Data.Functions.Commands.PaymentRdsStatistics
{
    public class PaymentRdsStatisticsCommand : IAsyncRequest<PaymentRdsStatisticsCommandResponse>, IAsyncRequest<PaymentRdsStatisticsCommandHandler>, IStatisticsCommand<PaymentStatisticsModel, RdsStatisticsForPaymentsModel>
    {
        public PaymentStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForPaymentsModel RdsStatisticsModel { get; set; }
    }
}
