using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Infrastructure.Data;

namespace SFA.DAS.Data.AcceptanceTests.Data
{ 
    public class TransferTestsRepository : TestsRepositoryBase
    {
     

        public TransferTestsRepository(string connectionString) : base(connectionString)
        {
           
        }

        public async Task DeleteTransferRelationships()
        {
            await WithConnection(async c =>
            {
                return await c.ExecuteAsync(
                    sql: "TRUNCATE TABLE [Data_Load].[DAS_Employer_Transfer_Relationships]",
                    commandType: CommandType.Text);
            });

          
        }

        public async Task<int> GetNumberOfSentTransferRelationships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Transfer_Relationships] where [SenderUserId] <> 0",
                    commandType: CommandType.Text)
            );
        }

        public async Task<int> GetNumberOfApprovedTransferRelationships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Transfer_Relationships] where [ApproverUserId] <> 0",
                    commandType: CommandType.Text)
            );
        }
        public async Task<int> GetNumberOfRejectedTransferRelationships()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>(
                    sql: "SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Transfer_Relationships] where [RejectorUserId] <> 0",
                    commandType: CommandType.Text)
            );
        }
    }
}
