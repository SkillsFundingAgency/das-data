
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SFA.DAS.Data.DatabaseTests.TestHelpers
{
    public class ExecuteHelper : HelperBase
    {
        
        public ExecuteHelper(string connectionString) : base(connectionString)
        {
        }
        
        public async Task<IEnumerable<dynamic>> ExecuteView(string view)
        {
            return await WithConnection(async c =>
            {
                return await c.QueryAsync(
                    sql: $"Select * from {view}",
                    commandType: CommandType.Text);
            });
        }
    }
}
