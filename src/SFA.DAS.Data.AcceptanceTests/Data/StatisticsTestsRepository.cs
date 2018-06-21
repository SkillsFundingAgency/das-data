using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class StatisticsTestsRepository : TestsRepositoryBase
    {
        public StatisticsTestsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task DeleteConsistencyChecks()
        {
            await WithConnection(async c => await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_ConsistencyCheck]",
                commandType: CommandType.Text));
        }

        public async Task InsertCommitmentsData(CommitmentsRecord commitmentsRecord)
        {
            await WithConnection(async c => await c.InsertAsync(commitmentsRecord));
        }

        public async Task InsertPaymentsData(PaymentsRecord paymentsRecord)
        {
            await WithConnection(async c => await c.InsertAsync(paymentsRecord));
        }

        public async Task DeleteCommitments()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Commitments]", commandType: CommandType.Text));
        }

        public async Task DeletePayments()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Payments]", commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfLatestTotalCohorts()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_ConsistencyCheck] WHERE IsLatest=1 AND DataType='TotalCohorts'",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfLatestTotalApprenticeships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_ConsistencyCheck] WHERE IsLatest=1 AND DataType='TotalApprenticeships'",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfLatestActiveApprenticeships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_ConsistencyCheck] WHERE IsLatest=1 AND DataType='ActiveApprenticeships'",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfProviderTotalPayments()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Payments]",
                    commandType: CommandType.Text));
        }
    }
}
