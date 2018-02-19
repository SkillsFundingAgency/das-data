using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.VerifyRelationship
{
    public class VerifyRelationshipCommandHandler : IAsyncNotificationHandler<VerifyRelationshipCommand>
    {
        private readonly IRelationshipRepository _relationshipRepository;

        public VerifyRelationshipCommandHandler(IRelationshipRepository relationshipRepository)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task Handle(VerifyRelationshipCommand notification)
        {
            await _relationshipRepository.VerifyRelationship(notification.ProviderId, notification.EmployerAccountId,
                notification.LegalEntityId, notification.Verified);
        }
    }
}
