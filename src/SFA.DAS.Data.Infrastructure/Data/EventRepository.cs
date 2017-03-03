using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<long> GetLastProcessedEventId(string eventFeed)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventFeed", eventFeed, DbType.String);

                return await c.QuerySingleOrDefaultAsync<long>(
                    sql: "[Data_Load].[GetLastProcessedEventId]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
            
            return result;
        }

        public async Task StoreLastProcessedEventId(string eventFeed, long id)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventFeed", eventFeed, DbType.String);
                parameters.Add("@lastProcessedEventId", id, DbType.Int64);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[StoreLastProcessedEventId]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<int> GetEventFailureCount(long eventId)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventId", eventId, DbType.Int64);

                return await c.QueryAsync<int>(
                    sql: "[Data_Load].[GetEventFailureCount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });

            return result.SingleOrDefault();
        }

        public async Task SetEventFailureCount(long eventId, int failureCount)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventId", eventId, DbType.Int64);
                parameters.Add("@failureCount", failureCount, DbType.Int32);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SetEventFailureCount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
