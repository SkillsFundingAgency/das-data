using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class CommitmentsRelationshipService : ICommitmentsRelationshipService
    {
        private readonly ICommitmentsRelationshipRepository _commitmentsRelationshipRepository;

        public CommitmentsRelationshipService(ICommitmentsRelationshipRepository commitmentsRelationshipRepository)
        {
            _commitmentsRelationshipRepository = commitmentsRelationshipRepository;
        }

        public void SaveCreatedRelationship(RelationshipCreated message)
        {
            var relationship = new Relationship
            {
                Id =  message.Relationship.Id,
                EmployerAccountId = message.Relationship.EmployerAccountId,
                LegalEntityId = message.Relationship.LegalEntityId,
                LegalEntityName = message.Relationship.LegalEntityName,
                LegalEntityAddress = message.Relationship.LegalEntityAddress,
                LegalEntityOrganisationType = message.Relationship.LegalEntityOrganisationType,
                ProviderId = message.Relationship.ProviderId,
                ProviderName = message.Relationship.ProviderName,
                Verified = message.Relationship.Verified
            };

            _commitmentsRelationshipRepository.CreateCommitmentsRelationship(relationship);
        }

        public void SaveVerifiedRelationship(RelationshipVerified message)
        {
            _commitmentsRelationshipRepository.VerifyCommitmentsRelationship(message.ProviderId, message.EmployerAccountId,
                message.LegalEntityId, message.Verified);
        }
    }
}
