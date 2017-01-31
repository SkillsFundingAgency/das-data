using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class LegalEntityRepository : BaseRepository, ILegalEntityRepository 
    {
        public LegalEntityRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveLegalEntity(LegalEntityViewModel legalEntity)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dasAccountId", legalEntity.DasAccountId, DbType.String);
                parameters.Add("@dasLegalEntityId", legalEntity.LegalEntityId, DbType.Int64);
                parameters.Add("@name", legalEntity.Name, DbType.String);
                parameters.Add("@address", legalEntity.Address, DbType.String);
                parameters.Add("@source", legalEntity.Source, DbType.String);
                parameters.Add("@inceptionDate", legalEntity.DateOfInception, DbType.DateTime);
                parameters.Add("@code", legalEntity.Code, DbType.String);
                parameters.Add("@status", legalEntity.Status, DbType.String);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveLegalEntity]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
