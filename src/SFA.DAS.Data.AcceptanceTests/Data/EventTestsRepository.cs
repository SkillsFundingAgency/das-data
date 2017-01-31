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
                    sql: "DELETE FROM [Data_Load].[DAS_Employer_Accounts]",
                    commandType: CommandType.Text);
            });

            await DeleteLegalEntities();
            await DeletePayeSchemes();
        }

        public async Task DeleteFailedEvents()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "DELETE FROM [Data_Load].[DAS_FailedEvents]",
                    commandType: CommandType.Text);
            });
        }

        public Task StoreLastProcessedEventId(string eventFeed, long id)
        {
            return _eventRepository.StoreLastProcessedEventId(eventFeed, id);
        }

        public Task<long> GetLastProcessedEventId(string eventFeed)
        {
            return _eventRepository.GetLastProcessedEventId(eventFeed);
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

        private async Task DeletePayeSchemes()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "DELETE FROM [Data_Load].[DAS_Employer_PayeSchemes]",
                    commandType: CommandType.Text);
            });
        }

        private async Task DeleteLegalEntities()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "DELETE FROM [Data_Load].[DAS_Employer_LegalEntities]",
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
