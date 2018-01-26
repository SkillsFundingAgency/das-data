using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[HMRC].[Data_Staging]")]
    public class DataStagingRecord
    {
        [Key]
        public long Record_ID { get; set; }

        public string SchemePAYERef { get; set; }

        public long SourceFile_ID { get; set; }

        public string CessationDate { get; set; }
    }
}