using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class AccountRepository : BaseRepository, IAccountRepository 
    {
        public AccountRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveAccount(AccountDetailViewModel registration)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dasAccountName", registration.DasAccountName, DbType.String);
                parameters.Add("@dateRegistered", registration.DateRegistered, DbType.DateTime);
                parameters.Add("@ownerEmail", registration.OwnerEmail, DbType.String);
                parameters.Add("@hashedAccountId", registration.HashedAccountId, DbType.String);
                parameters.Add("@accountId", registration.AccountId, DbType.Int64);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveAccount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<long> GetTotalNumberOfAccounts()
        {
            return await WithConnection(async c =>
            {
                return await c.ExecuteScalarAsync<long>(
                    sql: "[PerformancePlatform].[GetNumberOfEmployerAccounts]",
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
