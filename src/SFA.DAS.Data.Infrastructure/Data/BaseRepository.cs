using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository()
        {
            _connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
    }
}
