using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface ITransferRelationshipService
    {

        void SaveSentMessage(SentTransferConnectionInvitationEvent message);
        void SaveRejectedMessage(RejectedTransferConnectionInvitationEvent message);
        void SaveApprovedMessage(ApprovedTransferConnectionInvitationEvent message);
    }
}
