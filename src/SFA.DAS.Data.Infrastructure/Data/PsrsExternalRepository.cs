using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models.PSRS;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class PsrsExternalRepository : BaseRepository, IPsrsExternalRepository
    {
        public PsrsExternalRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<ReportSubmitted>> GetSubmittedReports(DateTime lastRun)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@lastRun", lastRun, DbType.DateTime2);

            var reports = (await WithConnection(async ctx => await ctx.QueryAsync<ReportSubmitted>(@"
            WITH base as 
            (
            Select [EmployerId],[ReportingPeriod], [ReportingData],
            JSON_VALUE([ReportingData], '$.OrganisationName') AS OrganisationName, 
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Id') AS Question1,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[0].Answer'), '') AS Answer1_1,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[1].Answer'), '') AS Answer1_2,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[2].Answer'), '') AS Answer1_3,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Id') AS Question2,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[0].Answer'), '') AS Answer2_1,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[1].Answer'), '') AS Answer2_2,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[2].Answer'), '') AS Answer2_3,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[2].Id') AS Question3,
            NULLIF(JSON_VALUE([ReportingData], '$.Questions[0].SubSections[2].Questions[0].Answer'), '') AS Answer3_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[0].Questions[0].Id') AS Question4_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[0].Title') AS QuestionText4_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[0].Questions[0].Answer') AS Answer4_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[1].Questions[0].Id') AS Question5_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[1].Title') AS QuestionText5_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[1].Questions[0].Answer') AS Answer5_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[2].Questions[0].Id') AS Question6_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[2].Title') AS QuestionText6_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[2].Questions[0].Answer') AS Answer6_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[3].Questions[0].Id') AS Question7_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[3].Title') AS QuestionText7_1,
            JSON_VALUE([ReportingData], '$.Questions[1].SubSections[3].Questions[0].Answer') AS Answer7_1,
            CONVERT(datetime2, JSON_VALUE([ReportingData], '$.Submitted.SubmittedAt')) AS SubmittedAt,
            JSON_VALUE([ReportingData], '$.Submitted.SubmittedName') AS SubmittedName,
            JSON_VALUE([ReportingData], '$.Submitted.SubmittedEmail') AS SubmittedEmail,
			JSON_VALUE([ReportingData], '$.ReportingPercentages.EmploymentStarts') AS ReportingEmploymentStarts,
			JSON_VALUE([ReportingData], '$.ReportingPercentages.TotalHeadCount') AS ReportingTotalHeadCount,
			JSON_VALUE([ReportingData], '$.ReportingPercentages.NewThisPeriod') AS ReportingNewThisPeriod,
            Submitted
            From [dbo].[Report]
            WHERE JSON_VALUE([ReportingData], '$.OrganisationName') IS NOT NULL
            )
            SELECT  a.[EmployerId] DasAccountId,
            a.OrganisationName,
            a.ReportingPeriod,
            a.FigureA,
            a.FigureB,
			ROUND(CONVERT(decimal(9, 4), a.ReportingEmploymentStarts) / cast(100 as decimal), 4) AS FigureE,
            a.FigureC,
            a.FigureD,
			ROUND(CONVERT(decimal(9, 4), a.ReportingTotalHeadCount) / cast(100 as decimal), 4) AS FigureF,
            a.FigureG,
            a.FigureH,
			ROUND(CONVERT(decimal(9, 4), a.ReportingNewThisPeriod) / cast(100 as decimal), 4) AS FigureI,
            NULLIF(a.Answer3_1, '') AS FullTimeEquivalent,
            a.Answer4_1 OutlineActions,
            a.Answer5_1 Challenges,
            a.Answer6_1 TargetPlans,
            ISNULL(a.Answer7_1,'') AnythingElse,
            a.SubmittedAt, a.SubmittedName, a.SubmittedEmail
            FROM (
            SELECT 
            CAST(CASE WHEN base.Answer1_3 IS NULL THEN 0 ELSE base.Answer1_3 END AS FLOAT) AS FigureA,
            CAST(CASE WHEN base.Answer2_3 IS NULL THEN 0 ELSE base.Answer2_3 END AS FLOAT) AS FigureB,
            CAST(CASE WHEN base.Answer1_2 IS NULL THEN 0 ELSE base.Answer1_2 END AS FLOAT) AS FigureC,
            CAST(CASE WHEN base.Answer2_2 IS NULL THEN 0 ELSE base.Answer2_2 END AS FLOAT) AS FigureD,
            CAST(CASE WHEN base.Answer2_1 IS NULL THEN 0 ELSE base.Answer2_1 END AS FLOAT) AS FigureG,
            CAST(CASE WHEN base.Answer1_1 IS NULL THEN 0 ELSE base.Answer1_1 END AS FLOAT) AS FigureH,
            base.*
            FROM base
            ) as a
            WHERE a.Submitted = 1 AND a.SubmittedAt > @lastRun
            ORDER BY a.SubmittedAt,a.EmployerId,a.ReportingPeriod
            ",
            param: parameters
            ))).ToList();

            reports.ForEach(x =>
                {
                    x.OutlineActionsWordCount = CountWords(x.OutlineActions);
                    x.AnythingElseWordCount = CountWords(x.AnythingElse);
                    x.ChallengesWordCount = CountWords(x.Challenges);
                    x.TargetPlansWordCount = CountWords(x.TargetPlans);
                }
            );

            return reports;
        }

        private int CountWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }
    
    
        public async Task<ReportSubmissionsSummary> GetSubmissionsSummary()
        {
            return await WithConnection(async ctx => await ctx.QuerySingleAsync<ReportSubmissionsSummary>(@"SELECT CONVERT (CHAR(12), CURRENT_TIMESTAMP, 106) ToDate,
            SUM(CASE WHEN Submitted = 1 THEN 1 ELSE 0 END) SubmittedTotals,
            SUM(CASE WHEN Submitted = 0 AND JSON_VALUE([ReportingData], '$.OrganisationName') IS NOT NULL THEN 1 ELSE 0 END) InProcessTotals,
            SUM(CASE WHEN Submitted = 0 AND JSON_VALUE([ReportingData], '$.OrganisationName') IS NULL THEN 1 ELSE 0 END) ViewedTotals,
            COUNT(*) Total,
            '1718' ReportingPeriod
            FROM[dbo].[Report]  "));
        }
    }
}
