using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IStatisticsRepository
    {
        Task<RdsStatisticsForEasModel> RetrieveEquivalentEasStatisticsFromRds();

        Task<RdsStatisticsForCommitmentsModel> RetrieveEquivalentCommitmentsStatisticsFromRds();

        Task<RdsStatisticsForPaymentsModel> RetrieveEquivalentPaymentStatisticsFromRds();

        Task SaveEasStatistics(EasStatisticsModel easStatisticsModel,
            RdsStatisticsForEasModel rdsStatisticsForEasModel);

        Task SaveCommitmentStatistics(CommitmentsStatisticsModel statisticsModel,
            RdsStatisticsForCommitmentsModel rdsModel);

        Task SavePaymentStatistics(PaymentStatisticsModel statisticsModel, RdsStatisticsForPaymentsModel rdsModel);
    }
}
