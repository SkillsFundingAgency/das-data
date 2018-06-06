using SFA.DAS.Commitments.Events;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface ICommitmentsRelationshipService
    {
        void SaveCreatedRelationship(RelationshipCreated message);
        void SaveVerifiedRelationship(RelationshipVerified message);
    }
}
