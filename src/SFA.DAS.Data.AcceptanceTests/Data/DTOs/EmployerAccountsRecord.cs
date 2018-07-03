using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_Employer_Accounts]")]
    public class EmployerAccountsRecord
    {
        public long Id { get; set; }
        public string DasAccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime DateRegistered { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public long AccountId { get; set; }
        public bool IsLatest { get; set; }
    }
}
