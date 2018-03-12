using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[HMRC].[Data_Quality_Tests_Log]")]
    public class QualityLog
    {
        [Key]
        public long Record_ID { get; set; }
        public string ColumnName { get; set; }
        public string TestName { get; set; }
        public string ErrorMessage { get; set; }
        public int FlagStopLoad { get; set; }
        public long SourceFile_ID { get; set; }
    }
}