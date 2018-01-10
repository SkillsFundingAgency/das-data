using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Infrastructure.Data;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class EventTestsRepository
    {
        private readonly string _connectionString;
        private readonly EventRepository _eventRepository;

        public EventTestsRepository(string connectionString)
        {
            _connectionString = connectionString;
            _eventRepository = new EventRepository(connectionString);
        }

        public async Task DeleteAccounts()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_Accounts]",
                    commandType: CommandType.Text);
            });

            await DeleteLegalEntities();
            await DeletePayeSchemes();
        }

        public async Task DeleteApprenticeships()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Commitments]",
                    commandType: CommandType.Text);
            });
        }

        public async Task DeletePayments()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Payments]",
                    commandType: CommandType.Text);
            });
        }

        public async Task DeleteLevyDeclarations()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_LevyDeclarations]",
                    commandType: CommandType.Text);
            });
        }

        public async Task DeleteEmployerAgreements()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_Agreements]",
                    commandType: CommandType.Text);
            });
        }

        public async Task DeleteEmploymentChecks()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_EmploymentCheck]",
                    commandType: CommandType.Text);
            });
        }

        public async Task DeleteFailedEvents()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_FailedEvents]",
                    commandType: CommandType.Text);
            });
        }

        public Task StoreLastProcessedEventId<T>(string eventFeed, T id)
        {
            return _eventRepository.StoreLastProcessedEventId(eventFeed, id);
        }

        public Task<T> GetLastProcessedEventId<T>(string eventFeed)
        {
            return _eventRepository.GetLastProcessedEventId<T>(eventFeed);
        }

        public async Task<int> GetNumberOfAccounts()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Accounts]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfLegalEntities()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_LegalEntities]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfPayeSchemes()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_PayeSchemes]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfApprenticeships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Commitments]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfPayments()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Payments]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfLevyDeclarations()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_LevyDeclarations]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfEmployerAgreements()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Agreements]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfEmploymentChecks()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_EmploymentCheck]",
                    commandType: CommandType.Text)
            );
        }

        private async Task DeletePayeSchemes()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_PayeSchemes]",
                    commandType: CommandType.Text);
            });
        }

        private async Task DeleteLegalEntities()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_LegalEntities]",
                    commandType: CommandType.Text);
            });
        }

        private async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
    }
}
