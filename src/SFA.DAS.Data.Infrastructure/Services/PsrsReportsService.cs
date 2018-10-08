using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class PsrsReportsService : IPsrsReportsService
    {
        private readonly IPsrsExternalRepository _psrsExternalRepository;
        private readonly IPsrsRepository _psrsRepository;

        public PsrsReportsService(IPsrsExternalRepository psrsExternalRepository, IPsrsRepository psrsRepository)
        {
            _psrsExternalRepository = psrsExternalRepository;
            _psrsRepository = psrsRepository;
        }

        public Task CreatePsrsReportSubmissionsSummary()
        {
            var summary = _psrsExternalRepository.GetSubmissionsSummary().Result;
            _psrsRepository.SaveSubmissionsSummary(summary);

            return Task.CompletedTask;
        }

        public Task CreatePsrsSubmittedReports()
        {
            var lastRun = _psrsRepository.GetLastSubmissionTime().Result;
            return CreatePsrsSubmittedReports(lastRun);
        }

        public Task CreatePsrsSubmittedReports(TimeSpan timeSpan)
        {
            var lastRun = DateTime.Now.Subtract(timeSpan);
            return CreatePsrsSubmittedReports(lastRun);
        }

        public Task CreatePsrsSubmittedReports(DateTime since)
        {
            var reports = _psrsExternalRepository.GetSubmittedReports(since).Result;
            _psrsRepository.SaveSubmittedReport(reports);

            return Task.CompletedTask;
        }
    }
}
