using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        public TransactionRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveTransaction(TransactionViewModel transaction)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DasAccountId", transaction.HashedAccountId, DbType.String);
                parameters.Add("@DateCreated", transaction.DateCreated, DbType.DateTime);
                parameters.Add("@SubmissionId", transaction.SubmissionId, DbType.Int64);
                parameters.Add("@TransactionDate", transaction.TransactionDate, DbType.DateTime);
                parameters.Add("@TransactionType", transaction.TransactionType, DbType.String);
                parameters.Add("@LevyDeclared", transaction.LevyDeclared, DbType.Decimal);
                parameters.Add("@Amount", transaction.Amount, DbType.Decimal);
                parameters.Add("@PayeSchemeRef", transaction.PayeSchemeRef, DbType.String);
                parameters.Add("@PeriodEnd", transaction.PeriodEnd, DbType.String);
                parameters.Add("@UkPrn", transaction.UkPrn, DbType.Int64);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveTransaction]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
