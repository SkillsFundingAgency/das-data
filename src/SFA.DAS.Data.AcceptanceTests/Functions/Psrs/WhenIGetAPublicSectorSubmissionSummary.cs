using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models.PSRS;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Psrs
{
    [TestFixture]
    public class WhenIGetAPublicSectorSubmissionSummary : PsrsTestBase
    {
        private ReportSubmissionsSummary _reportSubmissionsSummaryModel;

        [Test]
        public async Task ThenSavePublicSectorSubmissionsSummary()
        {
            await SetupDatabase();

            _reportSubmissionsSummaryModel = new ReportSubmissionsSummary
            {
                ToDate = DateTime.UtcNow,
                ReportingPeriod = "1718",
                InProcessTotals = 1,
                ViewedTotals = 2,
                SubmittedTotals = 3,
                Total = 1,
                //TODO: Use correct total when MPD-2316
                //Total = 6,
            };

            PsrsExternalRepositoryMock.Setup(x => x.GetSubmissionsSummary())
                .Returns(Task.FromResult(_reportSubmissionsSummaryModel));

            await DAS.Data.Functions.Psrs.CreatePsrsReportSubmissionsSummaryFunction.Run(null, Log, PsrsReportsService);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var summaryRecord = (await PsrsTestsRepository.GetReportSubmissionsSummaries())
                .FirstOrDefault(s => s.IsLatest 
                                     && s.ReportingPeriod == _reportSubmissionsSummaryModel.ReportingPeriod);

            if (summaryRecord != null)
            {
                return summaryRecord.InProcessTotals == _reportSubmissionsSummaryModel.InProcessTotals
                    && summaryRecord.SubmittedTotals == _reportSubmissionsSummaryModel.SubmittedTotals
                    && summaryRecord.ViewedTotals == _reportSubmissionsSummaryModel.ViewedTotals
                    && summaryRecord.Total == _reportSubmissionsSummaryModel.Total;
            }

            return false;
        }
    }
}
