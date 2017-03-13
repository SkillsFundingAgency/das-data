using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Domain.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class PayeSchemeRepository : BaseRepository, IPayeSchemeRepository 
    {
        public PayeSchemeRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SavePayeScheme(PayeSchemeViewModel payeScheme)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dasAccountId", payeScheme.DasAccountId, DbType.String);
                parameters.Add("@ref", payeScheme.Ref, DbType.String);
                parameters.Add("@name", payeScheme.Name, DbType.String);
                parameters.Add("@addedDate", payeScheme.AddedDate, DbType.DateTime);
                parameters.Add("@removedDate", payeScheme.RemovedDate, DbType.DateTime);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SavePayeScheme]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
