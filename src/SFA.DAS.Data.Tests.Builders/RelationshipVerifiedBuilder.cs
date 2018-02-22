using SFA.DAS.Commitments.Events;

namespace SFA.DAS.Data.Tests.Builders
{
    public class RelationshipVerifiedBuilder
    {
        public RelationshipVerified Build()
        {
            return new RelationshipVerified
            {
                ProviderId = 1,
                EmployerAccountId = 2,
                LegalEntityId = "3",
                Verified = true
            };
        }
    }
}
