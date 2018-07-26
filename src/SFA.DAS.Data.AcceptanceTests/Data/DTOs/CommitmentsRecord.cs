using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[Das_Commitments]")]
    public class CommitmentsRecord
    {
        public long Id { get; set; }
        public long CommitmentID { get; set; }
        public string PaymentStatus { get; set; }
        public long ApprenticeshipID { get; set; }
        public string AgreementStatus { get; set; }
        public string ProviderID { get; set; }
        public string LearnerID { get; set; }
        public string EmployerAccountID { get; set; }
        public string TrainingTypeID { get; set; }
        public string TrainingID { get; set; }
        public DateTime TrainingStartDate { get; set; }
        public DateTime TrainingEndDate { get; set; }
        public decimal TrainingTotalCost { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string LegalEntityCode { get; set; }
        public string LegalEntityName { get; set; }
        public string LegalEntityOrganisationType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsLatest { get; set; }
        public long? TransferSenderAccountId { get; set; }
        public string TransferApprovalStatus { get; set; }
        public DateTime? TransferApprovalDate { get; set; }
    }
}
