using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateCommitmentApprenticeshipEntry
{
    public class CreateCommitmentApprenticeshipEntryCommandHandler : IAsyncRequestHandler<CreateCommitmentApprenticeshipEntryCommand, CreateCommitmentApprenticeshipEntryResponse>
    {
        private readonly ICommitmentApprenticeshipRepository _repository;

        public CreateCommitmentApprenticeshipEntryCommandHandler(ICommitmentApprenticeshipRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateCommitmentApprenticeshipEntryResponse> Handle(CreateCommitmentApprenticeshipEntryCommand command)
        {
            await _repository.Create(command.Event);

            return new CreateCommitmentApprenticeshipEntryResponse();
        }
    }
}
