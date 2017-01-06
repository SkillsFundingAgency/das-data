using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommandHandler : AsyncRequestHandler<CreateRegistrationCommand>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IRegistrationGateway _registrationGateway;

        public CreateRegistrationCommandHandler(IRegistrationRepository registrationRepository, IRegistrationGateway registrationGateway)
        {
            _registrationRepository = registrationRepository;
            _registrationGateway = registrationGateway;
        }

        protected override async Task HandleCore(CreateRegistrationCommand message)
        {
            var registrations = await _registrationGateway.GetRegistration(message.DasAccountId);
            foreach (var registration in registrations)
            {
                await _registrationRepository.SaveRegistration(registration);
            }
        }
    }
}
