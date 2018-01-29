using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[HMRC].[Load_Control]")]
    public class LoadControlRecord
    {
        [Key]
        public long SourceFile_ID { get; set; }
        public string SourceFile_Name{ get; set; }
        public string SourceFile_Status { get; set; }
        public DateTime InsertDate { get; set; }
        public bool Flag_LoadedSuccessfullyintoLiveTable { get; set; }
        public bool Flag_LoadedSuccessfullyintoHistoryTable { get; set; }
    }
}