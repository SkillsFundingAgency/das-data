using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class ProviderRepository : BaseRepository, IProviderRepository
    {
        public ProviderRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveProvider(Apprenticeships.Api.Types.Providers.Provider provider)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UkPrn", provider.Ukprn, DbType.Int64);
                parameters.Add("@IsHigherEducationInstitute", provider.IsHigherEducationInstitute, DbType.Boolean);
                parameters.Add("@ProviderName", provider.ProviderName, DbType.String);
                parameters.Add("@IsEmployerProvider", provider.IsEmployerProvider, DbType.Boolean);
                parameters.Add("@Phone", provider.Phone, DbType.String);
                parameters.Add("@Email", provider.Email, DbType.String);
                parameters.Add("@NationalProvider", provider.NationalProvider, DbType.Boolean);
                parameters.Add("@Website", provider.Website, DbType.String);
                parameters.Add("@EmployerSatisfaction", provider.EmployerSatisfaction, DbType.Decimal);
                parameters.Add("@LearnerSatisfaction", provider.LearnerSatisfaction, DbType.Decimal);

                await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveProvider]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                await SaveAddresses(c, provider);
                await SaveAliases(c, provider);

                return 0;
            });
        }

        private async Task SaveAddresses(IDbConnection connection, Apprenticeships.Api.Types.Providers.Provider provider)
        {
            foreach (var address in provider.Addresses)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UkPrn", provider.Ukprn, DbType.Int64);
                parameters.Add("@ContactType", address.ContactType, DbType.String);
                parameters.Add("@Primary", address.Primary, DbType.String);
                parameters.Add("@Secondary", address.Secondary, DbType.String);
                parameters.Add("@Street", address.Street, DbType.String);
                parameters.Add("@Town", address.Town, DbType.String);
                parameters.Add("@Postcode", address.PostCode, DbType.String);

                await connection.ExecuteAsync(
                    sql: "[Data_Load].[SaveProviderAddress]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        private async Task SaveAliases(IDbConnection connection, Apprenticeships.Api.Types.Providers.Provider provider)
        {
            foreach (var alias in provider.Aliases)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UkPrn", provider.Ukprn, DbType.Int64);
                parameters.Add("@Alias", alias, DbType.String);

                await connection.ExecuteAsync(
                    sql: "[Data_Load].[SaveProviderAlias]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
