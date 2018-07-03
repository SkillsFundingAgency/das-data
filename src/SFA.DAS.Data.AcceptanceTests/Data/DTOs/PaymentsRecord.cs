using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[Das_Payments]")]
    public class PaymentsRecord
    {
        public long Id { get; set; }
        public string PaymentId { get; set; }
        public long? UkPrn { get; set; }
        public long? Uln { get; set; }
        public string EmployerAccountId { get; set; }
        public long? ApprenticeshipId { get; set; }
        public int? DeliveryMonth { get; set; }
        public int? DeliveryYear { get; set; }
        public int? CollectionMonth { get; set; }
        public int? CollectionYear { get; set; }
        public DateTime? EvidenceSubmittedOn { get; set; }
        public string EmployerAccountVersion { get; set; }
        public string ApprenticeshipVersion { get; set; }
        public string FundingSource { get; set; }
        public string TransactionType { get; set; }
        public decimal? Amount { get; set; }
        public int? StandardCode { get; set; }
        public int? FrameworkCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? PathwayCode { get; set; }
        public string ContractType { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public long? FundingAccountId { get; set; }
    }
}
