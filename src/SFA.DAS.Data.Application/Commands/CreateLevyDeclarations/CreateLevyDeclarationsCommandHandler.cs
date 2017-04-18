using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Commands.CreateLevyDeclarations
{
    public class CreateLevyDeclarationsCommandHandler : IAsyncNotificationHandler<CreateLevyDeclarationsCommand>
    {
        private readonly ILevyDeclarationRepository _levyDeclarationRepository;
        private readonly IAccountGateway _accountGateway;
        
        public CreateLevyDeclarationsCommandHandler(ILevyDeclarationRepository levyDeclarationRepository, IAccountGateway accountGateway)
        {
            _levyDeclarationRepository = levyDeclarationRepository;
            _accountGateway = accountGateway;
        }

        public async Task Handle(CreateLevyDeclarationsCommand notification)
        {
            var levyDeclarations = await _accountGateway.GetLevyDeclarations(notification.LevyDeclarationsHref);
            await SaveLevyDeclarations(levyDeclarations);
        }

        private async Task SaveLevyDeclarations(List<LevyDeclarationViewModel> levyDeclarations)
        {
            var tasks = levyDeclarations.Select(x => _levyDeclarationRepository.SaveLevyDeclaration(x));
            await Task.WhenAll(tasks);
        }
    }
}
