using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class CommitmentsTestRepository : TestsRepositoryBase
    {
        public CommitmentsTestRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task DeleteCommitmentsRelationships()
        {
            await WithConnection(async ctx => await ctx.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Commitments_Relationships]",
                commandType: CommandType.Text));
        }

        public async Task<int> GetNumnerOfLatestCommitmentsRelationships()
        {
            return await WithConnection(async ctx =>
                await ctx.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Commitments_Relationships] WHERE IsLatest=1",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfLatestVerifiedCommitmentsRelationships()
        {
            return await WithConnection(async ctx =>
                await ctx.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Commitments_Relationships] WHERE IsLatest=1 AND Verified=1",
                    commandType: CommandType.Text));
        }

        public async Task InsertIntoCommitmentsRelationships(CommitmentsRelationshipsRecord commitmentsRelationshipsRecord)
        {
            await WithConnection(async c => await c.InsertAsync(commitmentsRelationshipsRecord));
        }
    }
}
