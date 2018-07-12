using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Enum;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

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
            var lastRun = DateTime.Now.AddYears(-1);
            var reports = _psrsExternalRepository.GetSubmittedReports(lastRun).Result;
            _psrsRepository.SaveSubmittedReport(reports);
            return Task.CompletedTask;

        }
    }
}
