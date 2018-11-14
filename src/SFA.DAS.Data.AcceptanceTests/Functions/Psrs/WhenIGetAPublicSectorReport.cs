using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models.PSRS;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Psrs
{
    [TestFixture]
    public class WhenIGetAPublicSectorReport : PsrsTestBase
    {
        private ReportSubmitted _reportSubmittedModel;

        [Test]
        public async Task ThenSavePublicSectorReport()
        {
            await SetupDatabase();

            _reportSubmittedModel = new ReportSubmitted
            {
                Id = 1001,
                DasAccountId = "EXYZ12",
                DasAccountName = "DAS Account",
                OrganisationName = "Organisation",
                ReportingPeriod = "1718",
                FigureA = 20,
                FigureB = 15,
                FigureE = .75M,
                FigureC = 30,
                FigureD = 25,
                FigureF = 0.3333M,
                FigureG = 40,
                FigureH = 35,
                FigureI = 0.4286M,
                FullTimeEquivalent = 5,
                OutlineActions = "Actions",
                OutlineActionsWordCount = 1,
                Challenges = "Getting things across the line",
                ChallengesWordCount = 5,
                TargetPlans = "Do it tomorrow",
                TargetPlansWordCount = 3,
                AnythingElse = "No nothing",
                AnythingElseWordCount = 2,
                SubmittedAt = DateTime.UtcNow,
                SubmittedName = "",
                SubmittedEmail = "",
            };

            PsrsExternalRepositoryMock.Setup(x => x.GetSubmittedReports(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new List<ReportSubmitted> { _reportSubmittedModel }.AsEnumerable()));

            await DAS.Data.Functions.Psrs.CreatePsrsSubmittedReportFunction.Run(null, Log, PsrsReportsService);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var submittedReport = (await PsrsTestsRepository.GetReportSubmitteds())
                .FirstOrDefault(s => s.IsLatest 
                                     && s.ReportingPeriod == _reportSubmittedModel.ReportingPeriod);

            if (submittedReport != null)
            {
                return submittedReport.DasAccountId == _reportSubmittedModel.DasAccountId
                    && submittedReport.OrganisationName == _reportSubmittedModel.OrganisationName
                    && submittedReport.ReportingPeriod == _reportSubmittedModel.ReportingPeriod
                    && submittedReport.FigureA == _reportSubmittedModel.FigureA
                    && submittedReport.FigureB == _reportSubmittedModel.FigureB
                    && submittedReport.FigureE == _reportSubmittedModel.FigureE
                    && submittedReport.FigureC == _reportSubmittedModel.FigureC
                    && submittedReport.FigureD == _reportSubmittedModel.FigureD
                    && submittedReport.FigureF == _reportSubmittedModel.FigureF
                    && submittedReport.FigureG == _reportSubmittedModel.FigureG
                    && submittedReport.FigureH == _reportSubmittedModel.FigureH
                    && submittedReport.FigureI == _reportSubmittedModel.FigureI
                    && submittedReport.FullTimeEquivalent == _reportSubmittedModel.FullTimeEquivalent
                    && submittedReport.OutlineActions == _reportSubmittedModel.OutlineActions
                    && submittedReport.OutlineActionsWordCount == _reportSubmittedModel.OutlineActionsWordCount
                    && submittedReport.Challenges == _reportSubmittedModel.Challenges
                    && submittedReport.ChallengesWordCount == _reportSubmittedModel.ChallengesWordCount
                    && submittedReport.TargetPlans == _reportSubmittedModel.TargetPlans
                    && submittedReport.TargetPlansWordCount == _reportSubmittedModel.TargetPlansWordCount
                    && submittedReport.AnythingElse == _reportSubmittedModel.AnythingElse
                    && submittedReport.AnythingElseWordCount == _reportSubmittedModel.AnythingElseWordCount
                    && submittedReport.SubmittedAt == _reportSubmittedModel.SubmittedAt
                    && submittedReport.SubmittedName == _reportSubmittedModel.SubmittedName
                    && submittedReport.SubmittedEmail == _reportSubmittedModel.SubmittedEmail;
            }

            return false;
        }
    }
}
