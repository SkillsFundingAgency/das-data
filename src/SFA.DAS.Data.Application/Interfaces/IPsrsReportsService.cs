using System;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IPsrsReportsService
    {
        Task CreatePsrsReportSubmissionsSummary();

        Task CreatePsrsSubmittedReports();

        Task CreatePsrsSubmittedReports(TimeSpan timespan);

        Task CreatePsrsSubmittedReports(DateTime since);
    }
}
