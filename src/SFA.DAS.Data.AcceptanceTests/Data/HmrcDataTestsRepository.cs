using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.Data
{
    public class HmrcDataTestsRepository : TestsRepositoryBase
    {
        public HmrcDataTestsRepository(string connectionString) : base(connectionString) {}

        public async Task DeleteStaging()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Data_Staging]");
        }

        public async Task DeleteLive()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Data_Live]");
        }

        public async Task DeleteHistory()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Data_History]");
        }

        public async Task ExecuteLoadData()
        {
            await WithConnection(async c =>
                await c.ExecuteAsync("[HMRC].[Load_Data]", commandType: CommandType.StoredProcedure));
        }

        public async Task DeleteLoadControl()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Load_Control]");
        }

        public async Task DeleteProcessLog()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Process_Log]");
        }

        public async Task DeleteQualityTests()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Configuration_Data_Quality_Tests]");
        }

        public async Task DeleteQualityLog()
        {
            await ExecuteAsync("TRUNCATE TABLE [HMRC].[Data_Quality_Tests_Log]");
        }

        public async Task InsertIntoLoadControl(LoadControlRecord loadControlRecord)
        {
            await WithConnection(async ctx =>
                await ctx.InsertAsync(loadControlRecord));
        }

        public async Task<IEnumerable<ProcessLogRecord>> GetProcessLog()
        {
            return await WithConnection(async c =>
                await c.QueryAsync<ProcessLogRecord>("SELECT * FROM [HMRC].[Process_Log]", commandType: CommandType.Text));
        }

        public async Task<int> GetDataLiveCount()
        {
            return await WithConnection(async c =>
                await c.QuerySingleAsync<int>("SELECT COUNT(*) FROM [HMRC].[Data_Live]", commandType: CommandType.Text));
        }

        public async Task InsertIntoStaging(DataStagingRecord stagingRecordRecord)
        {
            await WithConnection(
                async c =>
                    await c.InsertAsync(stagingRecordRecord)
            );
        }

        public async Task<IEnumerable<DataStagingRecord>> GetStagingRecords()
        {
            return await WithConnection(async c => await c.GetAllAsync<DataStagingRecord>());
        }

        public async Task<LoadControlRecord> GetLoadControl()
        {
            return await WithConnection(async c =>
                await c.QueryFirstAsync<LoadControlRecord>("SELECT TOP(1) * FROM [HMRC].[Load_Control]"));
        }

        public async Task InsertIntoHistory(DataHistoryRecord dataHistoryRecord)
        {
            await WithConnection(
                async c =>
                    await c.InsertAsync(dataHistoryRecord)
            );
        }

        public async Task InsertIntoDataQualityTests(DataQualityTestRecord dataQualityTestRecord)
        {
            await WithConnection(async c => await c.InsertAsync(dataQualityTestRecord));
        }

        public async Task<IEnumerable<QualityLog>> GetQualityLogs()
        {
            return await WithConnection(async c => await c.GetAllAsync<QualityLog>());
        }

     
    }
}