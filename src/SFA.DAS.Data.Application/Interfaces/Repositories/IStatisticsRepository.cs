using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IStatisticsRepository
    {
        Task<EasRdsModel> RetrieveEquivalentEasStatisticsFromRds();

        Task<CommitmentsRdsModel> RetrieveEquivalentCommitmentsStatisticsFromRds();

        Task<PaymentsRdsModel> RetrieveEquivalentPaymentStatisticsFromRds();

        Task SaveEasStatistics(EasExternalModel easStatisticsModel,
            EasRdsModel rdsStatisticsForEasModel);

        Task SaveCommitmentStatistics(CommitmentsExternalModel statisticsModel,
            CommitmentsRdsModel rdsModel);

        Task SavePaymentStatistics(PaymentExternalModel statisticsModel, PaymentsRdsModel rdsModel);
    }
}
