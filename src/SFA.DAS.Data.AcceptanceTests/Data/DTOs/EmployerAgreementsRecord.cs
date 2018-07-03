using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_Employer_Agreements]")]
    public class EmployerAgreementsRecord
    {
        public long Id { get; set; }
        public string DasAccountId { get; set; }
        public string Status { get; set; }
        public string SignedBy { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long DasLegalEntityId { get; set; }
        public string DasAgreementId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool IsLatest { get; set; }
    }
}
