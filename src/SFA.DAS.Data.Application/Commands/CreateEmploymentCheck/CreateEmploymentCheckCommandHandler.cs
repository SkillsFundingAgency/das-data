using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateEmploymentCheck
{
    public class CreateEmploymentCheckCommandHandler : IAsyncNotificationHandler<CreateEmploymentCheckCommand>
    {
        private readonly IEmploymentCheckRepository _repository;

        public CreateEmploymentCheckCommandHandler(IEmploymentCheckRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateEmploymentCheckCommand command)
        {
            await _repository.SaveEmploymentCheck(command.Event);
        }
    }
}
