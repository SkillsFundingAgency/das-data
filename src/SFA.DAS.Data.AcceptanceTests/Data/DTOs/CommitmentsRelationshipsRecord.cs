using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_Commitments_Relationships]")]
    public class CommitmentsRelationshipsRecord
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public long EmployerAccountId { get; set; }
        public string LegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public string LegalEntityAddress { get; set;} 
        public short LegalEntityOrganisationTypeId { get; set; }
        public string LegalEntityOrganisationTypeDescription { get; set; }
        public bool? Verified { get; set; }
        public bool IsLatest { get; set; }
    }
}
