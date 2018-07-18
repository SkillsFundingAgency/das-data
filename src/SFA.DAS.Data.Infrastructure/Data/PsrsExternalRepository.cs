using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            var reports = (await WithConnection(async ctx => await ctx.QueryAsync<ReportSubmitted>(@"
            WITH base as 
            (
            Select [Employerid],[ReportingPeriod], [ReportingData],
            '1 April 20' + SUBSTRING([ReportingPeriod],1,2) + ' to 31 March 20' + SUBSTRING([ReportingPeriod],3,2) AS ReportingPeriodLabel,
            JSON_VALUE([ReportingData], '$.OrganisationName') AS OrganisationName, 
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Id') AS Question1,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[0].Answer') AS Answer1_1,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[1].Answer') AS Answer1_2,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[2].Answer') AS Answer1_3,
            CASE JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].CompletionStatus') WHEN 2 THEN 'COMPLETED' ELSE '' END AS Completion1,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Id') AS Question2,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[0].Answer') AS Answer2_1,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[1].Answer') AS Answer2_2,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[2].Answer') AS Answer2_3,
            CASE JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].CompletionStatus') WHEN 2 THEN 'COMPLETED' ELSE '' END AS Completion2,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[2].Id') AS Question3,
            JSON_VALUE([ReportingData], '$.Questions[0].SubSections[2].Questions[0].Answer') AS Answer3_1,
            CASE JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].CompletionStatus') WHEN 2 THEN 'COMPLETED' ELSE '' END AS Completion3,
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
                   Submitted
            From [dbo].[Report]
            WHERE JSON_VALUE([ReportingData], '$.OrganisationName') IS NOT NULL
            )
            SELECT  a.[Employerid] DasAccountId,
            a.Organisation_Name,
            a.ReportingPeriod,
            a.FigureA,
            a.FigureB,
            FORMAT(ROUND(((a.FigureB)/ CONVERT(decimal, nullif(a.FigureA,0))),4),'######0.00') AS FigureE,
            a.FigureC,
            a.FigureD,
            FORMAT(ROUND(((a.FigureD)/ CONVERT(decimal, nullif(a.FigureC,0))),4),'######0.00') AS FigureF,
            a.FigureG,
            a.FigureH,
            FORMAT(ROUND(((a.FigureB)/ CONVERT(decimal, nullif(a.FigureH,0))),4),'######0.00') AS FigureI,
            a.Answer4_1 OutlineActions,
            a.Answer5_1 Challenges,
            a.Answer6_1 TargetPlans,
            ISNULL(a.Answer7_1,'') AnythingElse,
            a.SubmittedAt, a.SubmittedName, a.SubmittedEmail
            FROM (
            SELECT 
            [OrganisationName] AS Organisation_Name,
            [ReportingPeriodLabel] AS Reporting_Period,
            CAST(CASE WHEN base.Answer1_3 IS NULL THEN 0 ELSE base.Answer1_3 END AS FLOAT) AS FigureA,
            CAST(CASE WHEN base.Answer2_3 IS NULL THEN 0 ELSE base.Answer2_3 END AS FLOAT) AS FigureB,
            CAST(CASE WHEN base.Answer1_2 IS NULL THEN 0 ELSE base.Answer1_2 END AS FLOAT) AS FigureC,
            CAST(CASE WHEN base.Answer2_2 IS NULL THEN 0 ELSE base.Answer2_2 END AS FLOAT) AS FigureD,
            CAST(CASE WHEN base.Answer2_1 IS NULL THEN 0 ELSE base.Answer2_1 END AS FLOAT) AS FigureG,
            CAST(CASE WHEN base.Answer1_1 IS NULL THEN 0 ELSE base.Answer1_1 END AS FLOAT) AS FigureH,
            base.*
            FROM base
            ) as a
            WHERE a.Submitted = 1 and a.SubmittedAt > '" + lastRun.ToString("yyyy-MM-dd HH:mm:ss.fff") + @"'
            ORDER BY a.SubmittedAt,a.Employerid,a.ReportingPeriod
            "))).ToList();

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
