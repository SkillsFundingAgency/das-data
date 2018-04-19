using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Enum;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class TransferRelationshipMessageService : ITransferRelationshipService
    {
        private readonly IEventsApi _eventsApi;
        private readonly ITransferRelationshipRepository _transferRelationshipRepository;
        private static string connectionString =
          "Server=(localdb)\\ProjectsV13;Database=SFA.DAS.Data.Database;Integrated Security = true;Trusted_Connection=True;Pooling=False;Connect Timeout=30;MultipleActiveResultSets=True";

        public TransferRelationshipMessageService()
        {
            _transferRelationshipRepository = new TransferRelationshipRepository(connectionString);
        }

        public TransferRelationshipMessageService(ITransferRelationshipRepository transferRelationshipRepository)
        {
            _transferRelationshipRepository = transferRelationshipRepository;
        }

        public void SaveApprovedMessage(ApprovedTransferConnectionInvitationEvent message)
        {

            var transferRelationship = new TransferRelationship()
            {
                SenderAccountId = message.SenderAccountId,
                ReceiverAccountId = message.ReceiverAccountId,
                RelationshipStatus = TransferRelationshipStatus.Approved,
                ApproverUserId = message.ApprovedByUserId
            };

            _transferRelationshipRepository.SaveTransferRelationship(transferRelationship);

        }

        public void SaveRejectedMessage(RejectedTransferConnectionInvitationEvent message)
        {
            var transferRelationship = new TransferRelationship()
            {
                SenderAccountId = message.SenderAccountId,
                ReceiverAccountId = message.ReceiverAccountId,
                RelationshipStatus = TransferRelationshipStatus.Rejected,
                RejectorUserId = message.RejectorUserId
            };

            _transferRelationshipRepository.SaveTransferRelationship(transferRelationship);
        }

        public void SaveSentMessage(SentTransferConnectionInvitationEvent message)
        {

            var transferRelationship = new TransferRelationship()
            {
                SenderAccountId = message.SenderAccountId,
                ReceiverAccountId = message.ReceiverAccountId,
                RelationshipStatus = TransferRelationshipStatus.Pending,
                SenderUserId = message.SentByUserId
            };

            _transferRelationshipRepository.SaveTransferRelationship(transferRelationship);
        }
    }
}
