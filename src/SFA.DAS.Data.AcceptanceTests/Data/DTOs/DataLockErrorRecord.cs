using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[Das_DataLock_Errors]")]
    public class DataLockErrorRecord
    {
        public long Id { get; set; }
        public long DataLockId { get; set; }
        public string ErrorCode { get; set; }
        public string SystemDescription { get; set; }
    }
}
