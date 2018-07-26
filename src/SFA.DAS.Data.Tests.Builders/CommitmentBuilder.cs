using SFA.DAS.Commitments.Api.Types.Commitment;
using SFA.DAS.Common.Domain.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class CommitmentBuilder
    {
        public Commitment Build()
        {
            return new Commitment
            {
                Reference = "ABC",
                EmployerAccountId = 123,
                TransferSenderId = null,
                TransferSenderName = "ABC",
                LegalEntityId = null,
                LegalEntityName = null,
                LegalEntityAddress = null,
                LegalEntityOrganisationType = OrganisationType.Charities,
                ProviderId = null,
                ProviderName = "ABC"
            };
        }
    }
}
