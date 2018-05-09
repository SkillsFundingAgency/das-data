using MediatR;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry
{
    public class CreateCommitmentApprenticeshipEntryCommand : IAsyncRequest<CreateCommitmentApprenticeshipEntryResponse>
    {
        public ApprenticeshipEventView Event { get; set; }
    }
}
