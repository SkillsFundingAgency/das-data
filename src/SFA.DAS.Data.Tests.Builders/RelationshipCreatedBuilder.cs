using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using OrganisationType = SFA.DAS.Common.Domain.Types.OrganisationType;

namespace SFA.DAS.Data.Tests.Builders
{
    public class RelationshipCreatedBuilder
    {
        public RelationshipCreated Build()
        {
            return new RelationshipCreated(new Relationship()
            {
                EmployerAccountId = 1,
                Id = 1,
                LegalEntityAddress = "Legal Entity Address",
                LegalEntityId = "2",
                LegalEntityName = "Legal Entity Name",
                LegalEntityOrganisationType = OrganisationType.CompaniesHouse,
                ProviderId = 3,
                ProviderName = "Provider Name",
                Verified = true
            });
        }
    }
}
