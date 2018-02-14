using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IRelationshipRepository
    {
        Task CreateRelationship(Relationship relationship);
        Task VerifyRelationship(long providerId, long employerAccountId, string legalEntityId, bool? verified);
    }
}
