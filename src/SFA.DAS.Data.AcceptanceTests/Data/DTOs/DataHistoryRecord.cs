using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[HMRC].[Data_History]")]
    public class DataHistoryRecord
    {
        public long SourceFile_ID { get; set; }
        public string SchemePAYERef { get; set; }
        public long Record_ID { get; set; }
    }
}