using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class PerformancePlatformRepository : IPerformancePlatformRepository, IDisposable
    {
        private readonly string _connectionString;

        public PerformancePlatformRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection _connection;

        public async Task<long> GetNumberOfRecordsFromLastRun(string dataType)
        {
            var connection = GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@dataType", dataType, DbType.String);

            return await connection.QuerySingleOrDefaultAsync<long>(
                sql: "[PerformancePlatform].[GetLastRunStatistics]",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task CreateRunStatistics(string dataType, DateTime runDateTime, long numberOfRecords)
        {
            var connection = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@dataType", dataType, DbType.String);
            parameters.Add("@runDateTime", runDateTime, DbType.DateTime);
            parameters.Add("@numberOfRecords", numberOfRecords, DbType.Int64);

            await connection.ExecuteAsync(
                sql: "[PerformancePlatform].[CreateRunStatistics]",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        private SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }

            if (_connection.State == ConnectionState.Broken || _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
