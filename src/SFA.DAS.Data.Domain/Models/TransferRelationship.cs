using System;
using SFA.DAS.Data.Domain.Enum;

namespace SFA.DAS.Data.Domain.Models
{
    public class TransferRelationship
    {
       
        public long ReceiverAccountId { get; set; }


        public long SenderAccountId { get; set; }


        public TransferRelationshipStatus RelationshipStatus { get; set; }

        public long SenderUserId { get; set; }

        public long ApproverUserId { get; set; }

        public long RejectorUserId { get; set; }
    }
}
