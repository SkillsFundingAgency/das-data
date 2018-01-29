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
        public bool StopLoadIfTestTextLength { get; set; }
        public int ColumnPrecision { get; set; }
        public bool StopLoadIfTestDecimalPlaces { get; set; }
        public string ColumnPatternMatching { get; set; }
        public bool StopLoadIfTestPatternMatch { get; set; }
        public bool StopLoadIfTestIsNumeric { get; set; }
    }
}