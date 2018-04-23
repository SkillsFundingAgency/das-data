using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class TransferRelationshipRepository : BaseRepository, ITransferRelationshipRepository 
    {
        public TransferRelationshipRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<long> GetTransferRelationshipSenderUserId(long SenderAcountId, long ReceiverAccountId)
        {
            return await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SenderAccountId", SenderAcountId, DbType.Int64);
                parameters.Add("@ReceiverAccountId", ReceiverAccountId, DbType.Int64);

                return await c.QuerySingleOrDefaultAsync<long>(
                    sql: "[Data_Load].[GetSentTransferRelationship]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task SaveTransferRelationship(TransferRelationship transferRelationship)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SenderAccountId", transferRelationship.SenderAccountId, DbType.Int64);
                parameters.Add("@ReceiverAccountId", transferRelationship.ReceiverAccountId, DbType.Int64);
                parameters.Add("@RelationshipStatus", transferRelationship.RelationshipStatus, DbType.Int16);
                parameters.Add("@SenderUserId", transferRelationship.SenderUserId, DbType.Int64);
                parameters.Add("@ApproverUserId", transferRelationship.ApproverUserId, DbType.Int64);
                parameters.Add("@RejectorUserId", transferRelationship.RejectorUserId, DbType.Int64);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[CreateTransferRelationship]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
