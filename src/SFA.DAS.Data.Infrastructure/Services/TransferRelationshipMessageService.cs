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
       



        public TransferRelationshipMessageService(ITransferRelationshipRepository transferRelationshipRepository)
        {
            _transferRelationshipRepository = transferRelationshipRepository;
        }

        public void SaveApprovedMessage(ApprovedTransferConnectionInvitationEvent message)
        {
            var senderUserId = GetSenderUserId(message.SenderAccountId, message.ReceiverAccountId);

            var transferRelationship = new TransferRelationship()
            {
                SenderAccountId = message.SenderAccountId,
                ReceiverAccountId = message.ReceiverAccountId,
                SenderUserId = senderUserId,
                RelationshipStatus = TransferRelationshipStatus.Approved,
                ApproverUserId = message.ApprovedByUserId
            };

            _transferRelationshipRepository.SaveTransferRelationship(transferRelationship);

        }

        public void SaveRejectedMessage(RejectedTransferConnectionInvitationEvent message)
        {
            var senderUserId = GetSenderUserId(message.SenderAccountId, message.ReceiverAccountId);
            var transferRelationship = new TransferRelationship()
            {
                SenderAccountId = message.SenderAccountId,
                ReceiverAccountId = message.ReceiverAccountId,
                SenderUserId = senderUserId,
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

        private long GetSenderUserId(long SenderAccountId, long ReceiverAccountId)
        {
            return _transferRelationshipRepository.GetTransferRelationshipSenderUserId(SenderAccountId, ReceiverAccountId).Result;
        }
    }
}
