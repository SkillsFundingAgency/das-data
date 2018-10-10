using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class PsrsTestsRepository : TestsRepositoryBase
    {
        public PsrsTestsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task DeletePublicSectorReports()
        {
            await WithConnection(async c => await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_PublicSector_Reports]",
                commandType: CommandType.Text));
        }

        public async Task DeletePublicSectorSummary()
        {
            await WithConnection(async c => await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_PublicSector_Summary]",
                commandType: CommandType.Text));
        }

        public async Task<IEnumerable<ReportSubmissionsSummaryRecord>> GetReportSubmissionsSummaries()
        {
            return await WithConnection(async c =>
                await c.QueryAsync<ReportSubmissionsSummaryRecord>("SELECT * FROM [Data_Load].[DAS_PublicSector_Summary]", commandType: CommandType.Text));
        }

        public async Task<IEnumerable<ReportSubmittedRecord>> GetReportSubmitteds()
        {
            return await WithConnection(async c =>
                await c.QueryAsync<ReportSubmittedRecord>("SELECT * FROM [Data_Load].[DAS_PublicSector_Reports]", commandType: CommandType.Text));
        }
    }
}
