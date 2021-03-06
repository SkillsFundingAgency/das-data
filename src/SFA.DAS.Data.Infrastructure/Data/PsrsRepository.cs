﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models.PSRS;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class PsrsRepository : BaseRepository, IPsrsRepository
    {
        private ILog _log;

        public PsrsRepository(string connectionString, ILog log) : base(connectionString)
        {
            _log = log;
        }

        public async Task<DateTime> GetLastSubmissionTime()
        {
            var result = await WithConnection(async c =>
                await c.QuerySingleAsync<DateTime?>(
                    sql: "[Data_Load].[GetLastPublicSectorReportSubmittedTime]",
                    commandType: CommandType.StoredProcedure));

            return result ?? DateTime.MinValue;
        }

        public async Task SaveSubmittedReport(IEnumerable<ReportSubmitted> reports)
        {
            await WithConnection(async c =>
            {
                foreach (var report in reports)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@dasAccountId", report.DasAccountId, DbType.String);
                    parameters.Add("@organisationName", report.OrganisationName, DbType.String);
                    parameters.Add("@reportingPeriod", report.ReportingPeriod, DbType.Int32);
                    parameters.Add("@figureA", report.FigureA, DbType.Int32);
                    parameters.Add("@figureB", report.FigureB, DbType.Int32);
                    parameters.Add("@figureE", report.FigureE, DbType.Decimal);
                    parameters.Add("@figureC", report.FigureC, DbType.Int32);
                    parameters.Add("@figureD", report.FigureD, DbType.Int32);
                    parameters.Add("@figureF", report.FigureF, DbType.Decimal);
                    parameters.Add("@figureG", report.FigureG, DbType.Int32);
                    parameters.Add("@figureH", report.FigureH, DbType.Int32);
                    parameters.Add("@figureI", report.FigureI, DbType.Decimal);
                    parameters.Add("@fullTimeEquivalent", report.FullTimeEquivalent, DbType.Int32);
                    parameters.Add("@outlineActions", report.OutlineActions, DbType.String);
                    parameters.Add("@outlineActionsWordCount", report.OutlineActionsWordCount, DbType.Int32);
                    parameters.Add("@challenges", report.Challenges, DbType.String);
                    parameters.Add("@challengesWordCount", report.ChallengesWordCount, DbType.Int32);
                    parameters.Add("@targetPlans", report.TargetPlans, DbType.String);
                    parameters.Add("@targetPlansWordCount", report.TargetPlansWordCount, DbType.Int32);
                    parameters.Add("@anythingElse", report.AnythingElse, DbType.String);
                    parameters.Add("@anythingElseWordCount", report.AnythingElseWordCount, DbType.Int32);
                    parameters.Add("@@submittedAt", report.SubmittedAt, DbType.DateTime2);
                    parameters.Add("@submittedName", report.SubmittedName, DbType.String);
                    parameters.Add("@submittedEmail", report.SubmittedEmail, DbType.String);

                    try
                    {
                        await c.ExecuteAsync(
                            sql: "[Data_Load].[SavePublicSectorReports]",
                            param: parameters,
                            commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex, $"Unable to save PSR data for DasAccountId {report.DasAccountId} period {report.ReportingPeriod}");
                    }
                }
                return 0;
            });
        }

        public async Task SaveSubmissionsSummary(ReportSubmissionsSummary summary)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@submittedTotals", summary.SubmittedTotals, DbType.Int32);
                parameters.Add("@viewedTotals", summary.ViewedTotals, DbType.Int32);
                parameters.Add("@inProcessTotals", summary.InProcessTotals, DbType.Int32);
                parameters.Add("@total", summary.Total, DbType.Int32);
                parameters.Add("@reportingPeriod", summary.ReportingPeriod, DbType.String);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveSubmissionsSummary]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
