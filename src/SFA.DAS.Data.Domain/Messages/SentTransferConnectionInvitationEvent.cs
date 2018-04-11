using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.DAS.EmployerAccounts.Events.Messages
{
    [Serializable]
    public class SentTransferConnectionInvitationEvent
    {
        public DateTime CreatedAt { get; set; }
        public string ReceiverAccountHashedId { get; set; }
        public long ReceiverAccountId { get; set; }
        public string ReceiverAccountName { get; set; }
        public string SenderAccountHashedId { get; set; }
        public long SenderAccountId { get; set; }
        public string SenderAccountName { get; set; }
        public Guid SentByUserExternalId { get; set; }
        public long SentByUserId { get; set; }
        public string SentByUserName { get; set; }
        public int TransferConnectionInvitationId { get; set; }
    }
}