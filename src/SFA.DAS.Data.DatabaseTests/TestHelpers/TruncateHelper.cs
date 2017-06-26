using Dapper;
using System.Data;

using System.Threading.Tasks;

namespace SFA.DAS.Data.DatabaseTests.TestHelpers
{
    public class TruncateHelper : HelperBase
    {
        public TruncateHelper(string connectionString) : base(connectionString)
        {
        }

        public async Task TruncateAllTable()
        {
            foreach (var sqlcommand in tables)
            {
                await WithConnection(async c =>
                {
                    return await c.ExecuteAsync(
                        sql: $"TRUNCATE TABLE [Data_Load].{sqlcommand}",
                        commandType: CommandType.Text);
                });
            }
        }


    }
}
