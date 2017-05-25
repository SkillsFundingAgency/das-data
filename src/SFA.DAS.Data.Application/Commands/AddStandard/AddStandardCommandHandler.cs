using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.AddStandard
{
    public class AddStandardCommandHandler : IAsyncNotificationHandler<AddStandardCommand>
    {
        private readonly IStandardRepository _standardRepository;
        private readonly IStandardGateway _standardGateway;

        public AddStandardCommandHandler(IStandardRepository standardRepository, IStandardGateway standardGateway)
        {
            _standardRepository = standardRepository;
            _standardGateway = standardGateway;
        }

        public async Task Handle(AddStandardCommand notification)
        {
            var standard = await _standardGateway.GetStandard(notification.StandardId);
            await _standardRepository.SaveStandard(standard);
        }
    }
}
