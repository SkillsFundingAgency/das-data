using SFA.DAS.Commitments.Api.Types;
using OrganisationType = SFA.DAS.Common.Domain.Types.OrganisationType;

namespace SFA.DAS.Data.Tests.Builders
{
    public class RelationshipBuilder
    {
        public Relationship Build()
        {
            return new Relationship
            {
                EmployerAccountId = 2,
                Id = 1,
                LegalEntityAddress = "Legal Entity Address",
                LegalEntityId = "3",
                LegalEntityName = "Legal Entity Name",
                LegalEntityOrganisationType = OrganisationType.CompaniesHouse,
                ProviderId = 1,
                ProviderName = "Provider Name",
                Verified = false
            };
        }
    }
}
