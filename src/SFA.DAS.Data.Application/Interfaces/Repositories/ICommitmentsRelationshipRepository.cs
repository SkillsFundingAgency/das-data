using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface ICommitmentsRelationshipRepository
    {
        Task CreateCommitmentsRelationship(Relationship relationship);
        Task VerifyCommitmentsRelationship(long providerId, long employerAccountId, string legalEntityId, bool? verified);
    }
}
