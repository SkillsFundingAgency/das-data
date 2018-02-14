using MediatR;
using SFA.DAS.Commitments.Api.Types;

namespace SFA.DAS.Data.Application.Commands.VerifyRelationship
{
    public class VerifyRelationshipCommand : IAsyncNotification
    {
        public long ProviderId { get; set; }
        public long EmployerAccountId { get; set; }
        public string LegalEntityId { get; set; }
        public bool? Verified { get; set; }
    }
}
