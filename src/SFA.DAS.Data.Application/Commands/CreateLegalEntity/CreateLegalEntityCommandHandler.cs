using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateLegalEntity
{
    public class CreateLegalEntityCommandHandler : IAsyncNotificationHandler<CreateLegalEntityCommand>
    {
        private readonly ILegalEntityRepository _legalEntityRepository;
        private readonly IAccountGateway _accountGateway;
        
        public CreateLegalEntityCommandHandler(ILegalEntityRepository legalEntityRepository, IAccountGateway accountGateway)
        {
            _legalEntityRepository = legalEntityRepository;
            _accountGateway = accountGateway;
        }

        public async Task Handle(CreateLegalEntityCommand notification)
        {
            var legalEntity = await _accountGateway.GetLegalEntity(notification.LegalEntityHref);
            await _legalEntityRepository.SaveLegalEntity(legalEntity);
        }
    }
}
