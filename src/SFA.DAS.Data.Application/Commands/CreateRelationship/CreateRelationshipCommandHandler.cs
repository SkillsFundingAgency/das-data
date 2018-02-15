using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateRelationship
{
    public class CreateRelationshipCommandHandler : IAsyncNotificationHandler<CreateRelationshipCommand>
    {
        private readonly IRelationshipRepository _relationshipRepository;

        public CreateRelationshipCommandHandler(IRelationshipRepository relationshipRepository)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task Handle(CreateRelationshipCommand notification)
        {
            await _relationshipRepository.CreateRelationship(notification.Relationship);
        }
    }
}
