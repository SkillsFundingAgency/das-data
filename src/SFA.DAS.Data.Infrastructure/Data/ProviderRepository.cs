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

        public async Task SaveProvider(Roatp.Api.Types.Provider provider)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ukprn", provider.Ukprn, DbType.Int64);
                parameters.Add("@uri", provider.Uri, DbType.String);
                parameters.Add("@providerType", provider.ProviderType, DbType.Int32);
                parameters.Add("@parentCompanyGuarantee", provider.ParentCompanyGuarantee, DbType.Boolean);
                parameters.Add("@newOrganisationWithoutFinancialTrackRecord", provider.NewOrganisationWithoutFinancialTrackRecord, DbType.Boolean);
                parameters.Add("@startDate", provider.StartDate, DbType.DateTime );

                return await c.ExecuteAsync(
                    sql: "[RoATP].[SaveRoatpProvider]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
