using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.AddRoatpProvider
{
    public class AddRoatpProviderCommandHandler : IAsyncNotificationHandler<AddRoatpProviderCommand>
    {
        private readonly IRoatpRepository _roatpRepository;
        private readonly IRoatpGateway _roatpGateway;

        public AddRoatpProviderCommandHandler(IRoatpRepository roatpRepository, IRoatpGateway roatpGateway)
        {
            _roatpRepository = roatpRepository;
            _roatpGateway = roatpGateway;
        }

        public async Task Handle(AddRoatpProviderCommand notification)
        {
            var provider = await _roatpGateway.GetProvider(notification.ProviderId);
            await _roatpRepository.SaveRoatpProvider(provider);
        }
    }
}
