using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Infrastructure.Data;

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
        
        public async Task<int> GetNumberOfProviders()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[Provider]",
                    commandType: CommandType.Text)
            );
        }

        public async Task DeleteRelationships()
        {
            await WithConnection(async c => await c.ExecuteAsync(sql: "TRUNCATE TABLE [Data_Load].[DAS_Relationship]",
                commandType: CommandType.Text));
        }

        public async Task<int> GetNumberOfRelationships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Relationship]",
                    commandType: CommandType.Text));
        }

        public async Task InsertRelationship(Relationship relationship)
        {
            string verified;

            if (relationship.Verified.HasValue)
            {
                verified = relationship.Verified == true ? "1" : "0";
            }
            else
            {
                verified = "NULL";
            }

            var sql =
                $"INSERT INTO [Data_Load].[Das_Relationship] " +
                $"VALUES ( {relationship.ProviderId}, " +
                $"'{relationship.ProviderName}', " +
                $"{relationship.EmployerAccountId}, " +
                $"'{relationship.LegalEntityId}', " +
                $"'{relationship.LegalEntityName}', " +
                $"'{relationship.LegalEntityAddress}', " +
                $"{Convert.ToByte(relationship.LegalEntityOrganisationType)}, " +
                $"'{relationship.LegalEntityOrganisationType.ToString()}', " +
                $"{verified}, 1)";

            await WithConnection(async c => await c.ExecuteAsync(sql: sql, commandType: CommandType.Text));

        }

        public async Task<int> GetNumberOfVerifiedRelationships(RelationshipVerified relationshipVerified)
        {
            var sql = $"SELECT COUNT(*) FROM [Data_Load].[DAS_Relationship] " +
                         $"WHERE ProviderId={relationshipVerified.ProviderId} AND " +
                         $"EmployerAccountId={relationshipVerified.EmployerAccountId} AND " +
                         $"LegalEntityId='{relationshipVerified.LegalEntityId}' AND " +
                         $"Verified={relationshipVerified.Verified} ";

            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(sql: sql, commandType: CommandType.Text));

        }
    }
}
