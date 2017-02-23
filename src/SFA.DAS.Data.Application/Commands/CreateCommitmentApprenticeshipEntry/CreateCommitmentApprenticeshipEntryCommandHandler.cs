using System;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry
{
    public class CreateCommitmentApprenticeshipEntryCommandHandler : IAsyncRequestHandler<CreateCommitmentApprenticeshipEntryCommand, CreateCommitmentApprenticeshipEntryResponse>
    {
        public Task<CreateCommitmentApprenticeshipEntryResponse> Handle(CreateCommitmentApprenticeshipEntryCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
