using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models.PSRS;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IPsrsExternalRepository
    {
        Task<IEnumerable<ReportSubmitted>> GetSubmittedReports(DateTime lastRun);
        Task<ReportSubmissionsSummary> GetSubmissionsSummary();
    }
}
