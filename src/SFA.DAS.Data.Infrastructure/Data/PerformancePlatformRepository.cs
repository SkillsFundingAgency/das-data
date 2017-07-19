using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class PerformancePlatformRepository : BaseRepository, IPerformancePlatformRepository
    {
        public PerformancePlatformRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<long> GetNumberOfRecordsFromLastRun(string dataType)
        {
            return await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dataType", dataType, DbType.String);

                return await c.QuerySingleOrDefaultAsync<long>(
                    sql: "[PerformancePlatform].[GetLastRunStatistics]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task CreateRunStatistics(string dataType, DateTime runDateTime, long numberOfRecords)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dataType", dataType, DbType.String);
                parameters.Add("@runDateTime", runDateTime, DbType.DateTime);
                parameters.Add("@numberOfRecords", numberOfRecords, DbType.Int64);

                return await c.ExecuteAsync(
                    sql: "[PerformancePlatform].[CreateRunStatistics]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
