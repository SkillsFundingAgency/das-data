using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreateEmployerAgreement
{
    public class CreateEmployerAgreementCommandHandler : IAsyncNotificationHandler<CreateEmployerAgreementCommand>
    {
        private readonly IEmployerAgreementRepository _employerAgreementRepository;
        private readonly IAccountGateway _accountGateway;
        private readonly IMediator _mediator;

        public CreateEmployerAgreementCommandHandler(IEmployerAgreementRepository employerAgreementRepository, IAccountGateway accountGateway, IMediator mediator)
        {
            _employerAgreementRepository = employerAgreementRepository;
            _accountGateway = accountGateway;
            _mediator = mediator;
        }

        public async Task Handle(CreateEmployerAgreementCommand notification)
        {
            var employerAgreement = await _accountGateway.GetEmployerAgreement(notification.AgreementHref);
            await _employerAgreementRepository.SaveEmployerAgreement(employerAgreement);
        }
    }
}
