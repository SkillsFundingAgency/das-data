using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[HMRC].[Configuration_Data_Quality_Tests]")]
    public class DataQualityTestRecord
    {
        public string ColumnName { get; set; }
        public bool ColumnNullable { get; set; }
        public string ColumnType { get; set; }
        public int ColumnLength { get; set; }
        public bool RunColumnTests { get; set; }
        public bool FlagStopLoadIfTestTextLenght { get; set; }
    }
}