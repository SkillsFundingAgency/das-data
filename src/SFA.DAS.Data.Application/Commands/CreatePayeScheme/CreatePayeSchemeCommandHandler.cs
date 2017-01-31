using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.CreatePayeScheme
{
    public class CreatePayeSchemeCommandHandler : IAsyncNotificationHandler<CreatePayeSchemeCommand>
    {
        private readonly IPayeSchemeRepository _payeSchemeRepository;
        private readonly IAccountGateway _accountGateway;
        
        public CreatePayeSchemeCommandHandler(IPayeSchemeRepository payeSchemeRepository, IAccountGateway accountGateway)
        {
            _payeSchemeRepository = payeSchemeRepository;
            _accountGateway = accountGateway;
        }

        public async Task Handle(CreatePayeSchemeCommand notification)
        {
            var payeScheme = await _accountGateway.GetPayeScheme(notification.PayeSchemeHref);
            await _payeSchemeRepository.SavePayeScheme(payeScheme);
        }
    }
}
