using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class RoatpRepository : BaseRepository, IRoatpRepository
    {
        public RoatpRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveRoatpProvider(Roatp.Api.Types.Provider provider)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UkPrn", provider.Ukprn, DbType.Int64);
                parameters.Add("@ProviderType", provider.ProviderType.ToString(), DbType.String);
                parameters.Add("@ParentCompanyGuarantee", provider.ParentCompanyGuarantee, DbType.Boolean);
                parameters.Add("@NewOrganisationWithoutFinancialTrackRecord", provider.NewOrganisationWithoutFinancialTrackRecord, DbType.Boolean);
                parameters.Add("@StartDate", provider.StartDate, DbType.DateTime);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveRoatp]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
