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

        public async Task<T> GetLastProcessedEventId<T>(string eventFeed)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventFeed", eventFeed, DbType.String);

                return await c.QuerySingleOrDefaultAsync<T>(
                    sql: "[Data_Load].[GetLastProcessedEventId]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
            
            return result;
        }

        public async Task StoreLastProcessedEventId<T>(string eventFeed, T id)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventFeed", eventFeed, DbType.String);
                parameters.Add("@lastProcessedEventId", id, DbType.String);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[StoreLastProcessedEventId]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<int> GetEventFailureCount<T>(T eventId)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventId", eventId, DbType.String);

                return await c.QueryAsync<int>(
                    sql: "[Data_Load].[GetEventFailureCount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });

            return result.SingleOrDefault();
        }

        public async Task SetEventFailureCount<T>(T eventId, int failureCount)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventId", eventId, DbType.String);
                parameters.Add("@failureCount", failureCount, DbType.Int32);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SetEventFailureCount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
