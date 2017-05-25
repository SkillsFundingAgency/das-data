using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class StandardRepository : BaseRepository, IStandardRepository
    {
        public StandardRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveStandard(Standard standard)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StandardId", standard.StandardId, DbType.String);
                parameters.Add("@Title", standard.Title, DbType.String);
                parameters.Add("@Level", standard.Level, DbType.Int32);
                parameters.Add("@IsPublished", standard.IsPublished, DbType.Boolean);
                parameters.Add("@StandardPdf", standard.StandardPdf, DbType.String);
                parameters.Add("@AssessmentPlanPdf", standard.AssessmentPlanPdf, DbType.String);
                parameters.Add("@Duration", standard.Duration, DbType.Int32);
                parameters.Add("@MaxFunding", standard.MaxFunding, DbType.Int32);
                parameters.Add("@IntroductoryText", standard.IntroductoryText, DbType.String);
                parameters.Add("@EntryRequirements", standard.EntryRequirements, DbType.String);
                parameters.Add("@WhatApprenticesWillLearn", standard.WhatApprenticesWillLearn, DbType.String);
                parameters.Add("@Qualifications", standard.Qualifications, DbType.String);
                parameters.Add("@ProfessionalRegistration", standard.ProfessionalRegistration, DbType.String);
                parameters.Add("@OverviewOfRole", standard.OverviewOfRole, DbType.String);

                await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveStandard]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                await SaveJobRoles(c, standard);
                await SaveKeywords(c, standard);

                return 0;
            });
        }

        private async Task SaveJobRoles(IDbConnection connection, Standard standard)
        {
            foreach (var jobRole in standard.JobRoles)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StandardId", standard.StandardId, DbType.String);
                parameters.Add("@JobRole", jobRole, DbType.String);
                
                await connection.ExecuteAsync(
                    sql: "[Data_Load].[SaveStandardJobRole]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        private async Task SaveKeywords(IDbConnection connection, Standard standard)
        {
            foreach (var keyword in standard.Keywords)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StandardId", standard.StandardId, DbType.String);
                parameters.Add("@Keyword", keyword, DbType.String);

                await connection.ExecuteAsync(
                    sql: "[Data_Load].[SaveStandardKeyword]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
