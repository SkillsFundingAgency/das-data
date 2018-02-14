using MediatR;
using SFA.DAS.Commitments.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateRelationship
{
    public class CreateRelationshipCommand : IAsyncNotification
    {
        public Relationship Relationship;
    }
}
