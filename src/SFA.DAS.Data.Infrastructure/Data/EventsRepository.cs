using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class EventsRepository : BaseRepository, IEventRepository
    {
        public async Task<long> GetLastProcessedEventId(string eventFeed)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eventFeed", eventFeed, DbType.String);

                return await c.QuerySingleAsync<long>(
                    sql: "SELECT LastProcessedEventId FROM [Data_Load].[DAS_LoadedEvents] WHERE EventFeed = @eventFeed",
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
                    sql: "UPDATE [Data_Load].[DAS_LoadedEvents] SET LastProcessedEventId = @lastProcessedEventId " +
                         "WHERE EventFeed = @eventFeed",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }
    }
}
