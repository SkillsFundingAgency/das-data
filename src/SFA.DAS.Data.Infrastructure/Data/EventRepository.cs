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

                return await c.QuerySingleAsync<long>(
                    sql: "SELECT LastProcessedEventId FROM [Data_Load].[DAS_LoadedEvents] WHERE EventFeed = @eventFeed",
                    param: parameters,
                    commandType: CommandType.Text);
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
                    sql: "UPDATE [Data_Load].[DAS_LoadedEvents] SET LastProcessedEventId = @lastProcessedEventId " +
                         "WHERE EventFeed = @eventFeed",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }

        public async Task<int> GetEventFailureCount(long eventId)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventId", eventId, DbType.Int64);

                return await c.QueryAsync<int>(
                    sql: "SELECT FailureCount FROM [Data_Load].[DAS_FailedEvents] WHERE EventId = @eventId",
                    param: parameters,
                    commandType: CommandType.Text);
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
                    sql: "MERGE [Data_Load].[DAS_FailedEvents] AS [T] USING (SELECT @eventId AS EventId) AS [S] ON [T].EventId = [S].EventId " +
                         "WHEN MATCHED THEN UPDATE SET [T].FailureCount = @failureCount WHEN NOT MATCHED THEN INSERT(EventId, FailureCount) VALUES([S].EventId, @failureCount);",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }
    }
}
