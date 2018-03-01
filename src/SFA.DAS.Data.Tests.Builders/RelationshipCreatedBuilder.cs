using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using OrganisationType = SFA.DAS.Common.Domain.Types.OrganisationType;

namespace SFA.DAS.Data.Tests.Builders
{
    public class RelationshipCreatedBuilder : RelationshipBuilderBase
    {
        public RelationshipCreated Build()
        {
            return new RelationshipCreated(new Relationship
            {
                EmployerAccountId = Randomid ,
                Id = 1,
                LegalEntityAddress = LegalEntityAddress,
                LegalEntityId = Randomid.ToString(),
                LegalEntityName = LegalEntityName,
                LegalEntityOrganisationType = OrganisationType.CompaniesHouse,
                ProviderId = Randomid,
                ProviderName = ProviderName,
                Verified = false
            });
        }
    }
}
