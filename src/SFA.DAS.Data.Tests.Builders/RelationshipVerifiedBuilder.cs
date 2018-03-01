using SFA.DAS.Commitments.Events;

namespace SFA.DAS.Data.Tests.Builders
{
    public class RelationshipVerifiedBuilder : RelationshipBuilderBase
    {
        public RelationshipVerified Build()
        {
            return new RelationshipVerified
            {
                ProviderId = Randomid,
                EmployerAccountId = Randomid,
                LegalEntityId = Randomid.ToString(),
                Verified = true
            };
        }
    }
}
