using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

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
    }

    [Table("[HMRC].[Data_Staging]")]
    public class DataStagingRecord
    {
        [Key]
        public long Record_ID { get; set; }

        public string SchemePAYERef { get; set; }
    }

    [Table("[HMRC].[Load_Control]")]
    public class LoadControlRecord
    {
        [Key]
        public long SourceFile_ID { get; set; }
        public string SourceFile_Name{ get; set; }
        public string SourceFile_Status { get; set; }
        public DateTime InsertDate { get; set; }
    }

    public class ProcessLogRecord
    {
        public string ProcessEventName { get; set; }
        public string ProcessEventDescription { get; set; }
    }
}