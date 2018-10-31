using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class EventTestsRepository : TestsRepositoryBase
    {
        private readonly EventRepository _eventRepository;

        public EventTestsRepository(string connectionString) : base(connectionString)
        {
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

        public async Task DeleteTransfers()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_Account_Transfers]",
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

        public async Task DeleteProviders()
        {
            await WithConnection(async c => await c.ExecuteAsync(
                sql: "TRUNCATE TABLE [Data_Load].[Provider]",
                commandType: CommandType.Text));
        }

        public async Task DeleteDataLocks()
        {
            await WithConnection(async c =>
            {
                await c.ExecuteAsync(
                    sql: "DELETE FROM [Data_Load].[DAS_DataLock_Errors]",
                    commandType: CommandType.Text);
                await c.ExecuteAsync(
                    sql: "DELETE FROM [Data_Load].[DAS_DataLocks]",
                    commandType: CommandType.Text);

                return 0;
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

        public async Task<int> GetNumberOfDataLockEvents()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_DataLocks]",
                    commandType: CommandType.Text)
            );
        }
        
        public async Task<int> GetNumberOfDataLockErrors()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_DataLock_Errors]",
                    commandType: CommandType.Text)
            );
        }
       
        public async Task<IEnumerable<AccountTransfer>> GetTransfers()
        {
            return await WithConnection(async c =>
                await c.QueryAsync<AccountTransfer>(
                    sql: "SELECT * FROM [Data_Load].[DAS_Employer_Account_Transfers]",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfTransfers()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Account_Transfers]",
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
        
        public async Task<int> GetNumberOfProviders()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[Provider]",
                    commandType: CommandType.Text)
            );
        }
    }
}
