using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IStatisticsService
    {
        Task<IProcessingCompletedMessage> CollateEasMetrics();

        Task<IProcessingCompletedMessage> CollateCommitmentStatisticsMetrics(TraceWriter traceLog);

        Task<IProcessingCompletedMessage> CollatePaymentStatisticsMetrics();
    }
}
