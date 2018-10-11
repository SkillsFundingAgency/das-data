using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models.PSRS;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IPsrsRepository
    {
        Task<DateTime> GetLastSubmissionTime();

        Task SaveSubmittedReport(IEnumerable<ReportSubmitted> reports);

        Task SaveSubmissionsSummary(ReportSubmissionsSummary summary);
    }
}
