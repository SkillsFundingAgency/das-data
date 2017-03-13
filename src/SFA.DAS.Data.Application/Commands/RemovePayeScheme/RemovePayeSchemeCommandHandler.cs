using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Domain.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.RemovePayeScheme
{
    public class RemovePayeSchemeCommandHandler : IAsyncNotificationHandler<RemovePayeSchemeCommand>
    {
        private readonly IPayeSchemeRepository _payeSchemeRepository;
        private readonly IAccountGateway _accountGateway;
        
        public RemovePayeSchemeCommandHandler(IPayeSchemeRepository payeSchemeRepository, IAccountGateway accountGateway)
        {
            _payeSchemeRepository = payeSchemeRepository;
            _accountGateway = accountGateway;
        }

        public async Task Handle(RemovePayeSchemeCommand notification)
        {
            var payeScheme = await _accountGateway.GetPayeScheme(notification.PayeSchemeHref);
            await _payeSchemeRepository.SavePayeScheme(payeScheme);
        }
    }
}
