using SFA.DAS.Commitments.Api.Types;
using OrganisationType = SFA.DAS.Common.Domain.Types.OrganisationType;

namespace SFA.DAS.Data.Tests.Builders
{
    public class CommitmentsRelationshipBuilder
    {
        private long EmployerAccountId = 123;
        private string LegalEntityId = "123456789";
        private string LegalEntityName = "Test Legal Entity Name";
        private string LegalEntityAddress = "Test Legal Entity Address";
        private OrganisationType LegalEntityOrganisationType = OrganisationType.PublicBodies;
        private long ProviderId = 987654321;
        private string ProviderName = "Test Provider Name";

        public Relationship Build(bool verified)
        {
            return new Relationship
            {
                EmployerAccountId = EmployerAccountId,
                LegalEntityId = LegalEntityId,
                LegalEntityName = LegalEntityName,
                LegalEntityAddress = LegalEntityAddress,
                LegalEntityOrganisationType = LegalEntityOrganisationType,
                ProviderId = ProviderId,
                ProviderName = ProviderName,
                Verified = verified
            };
        }
    }
}
