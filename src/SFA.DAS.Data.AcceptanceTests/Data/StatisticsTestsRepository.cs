using System;
using System.Data;
using System.Data.SqlClient;
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

        public async Task InsertEmployerAccountsData(EmployerAccountsRecord employerAccountsRecord)
        {
            await WithConnection(async c => await c.InsertAsync(employerAccountsRecord));
        }

        public async Task InsertEmployerLegalEntities(EmployerLegalEntitiesRecord employerLegalEntities)
        {
            await WithConnection(async c => await c.InsertAsync(employerLegalEntities));
        }

        public async Task InsertEmployerPayeSchemes(EmployerPayeSchemesRecord employerPayeSchemes)
        {
            await WithConnection(async c => await c.InsertAsync(employerPayeSchemes));
        }

        public async Task InsertEmployerAgreements(EmployerAgreementsRecord employerAgreements)
        {
            await WithConnection(async c => await c.InsertAsync(employerAgreements));
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

        public async Task DeleteEmployerAccounts()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_Accounts]",
                    commandType: CommandType.Text));
        }

        public async Task DeleteEmployerLegalEntities()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_LegalEntities]",
                    commandType: CommandType.Text));
        }

        public async Task DeleteEmployerAgreements()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_Agreements]",
                    commandType: CommandType.Text));
        }

        public async Task DeleteEmployerPayeSchemes()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_PayeSchemes]",
                    commandType: CommandType.Text));
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

        public async Task<int> GetNumberOfTotalAccounts()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(Id) FROM [Data_Load].[DAS_Employer_Accounts] WHERE IsLatest = 1",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfTotalPayments()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(Id) AS TotalPayments FROM [Data_Load].[DAS_Payments] WHERE [CollectionYear] = YEAR(GETDATE())",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfTotalLegalEntities()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(Id) FROM [Data_Load].[DAS_Employer_LegalEntities] WHERE IsLatest = 1 AND [Status] = 'active'",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfTotalAgreements()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(Id) FROM [Data_Load].[DAS_Employer_Agreements] WHERE IsLatest = 1 AND [Status] = 'signed'",
                    commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfTotalPayeSchemes()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(Id) FROM [Data_Load].[DAS_Employer_PayeSchemes] WHERE IsLatest = 1",
                    commandType: CommandType.Text));
        }
    }
}
