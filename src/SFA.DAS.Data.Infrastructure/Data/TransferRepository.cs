using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class TransferRepository : BaseRepository, ITransferRepository
    {
        public TransferRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveTransfers(IEnumerable<AccountTransfer> transfers)
        {
            var parameters = new TransferTableValueParameter(transfers);

            await WithConnection(async con =>
            {
                await con.ExecuteAsync(
                    "[Data_Load].[SaveTransfers]",
                    parameters, 
                    commandType: CommandType.StoredProcedure);
                return 1L;
            });
        }
    }
}
