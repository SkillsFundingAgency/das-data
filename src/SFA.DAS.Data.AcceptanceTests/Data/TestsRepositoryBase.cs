using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class TestsRepositoryBase
    {
        private readonly string _connectionString;

        public TestsRepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }

        protected async Task ExecuteAsync(string sqlToExecute)
        {
            await WithConnection(async c => await c.ExecuteAsync(sqlToExecute, commandType: CommandType.Text));
        }
    }
}