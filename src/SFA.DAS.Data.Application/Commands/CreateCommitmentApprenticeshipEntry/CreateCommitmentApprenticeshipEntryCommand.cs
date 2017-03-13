using MediatR;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry
{
    public class CreateCommitmentApprenticeshipEntryCommand : IAsyncRequest<CreateCommitmentApprenticeshipEntryResponse>
    {
        public ApprenticeshipEvent Event { get; set; }
    }
}
