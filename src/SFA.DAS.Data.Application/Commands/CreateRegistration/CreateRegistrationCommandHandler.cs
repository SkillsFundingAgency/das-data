using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommandHandler : IAsyncNotificationHandler<CreateRegistrationCommand>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IRegistrationGateway _registrationGateway;

        public CreateRegistrationCommandHandler(IRegistrationRepository registrationRepository, IRegistrationGateway registrationGateway)
        {
            _registrationRepository = registrationRepository;
            _registrationGateway = registrationGateway;
        }

        public async Task Handle(CreateRegistrationCommand notification)
        {
            var registrations = await _registrationGateway.GetRegistration(notification.DasAccountId);
            foreach (var registration in registrations)
            {
                await _registrationRepository.SaveRegistration(registration);
            }
        }
    }
}
