using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.CreateRoatpProvider
{
    public class CreateProviderCommandHandler : IAsyncNotificationHandler<CreateProviderCommand>
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IRoatpGateway _roatpGateway;
        private readonly ILog _logger;

        public CreateProviderCommandHandler(IProviderRepository providerRepository, IRoatpGateway roatpGateway, ILog logger)
        {
            _providerRepository = providerRepository;
            _roatpGateway = roatpGateway;
            _logger = logger;
        }

        public async Task Handle(CreateProviderCommand command)
        {
            var provider = GetProvider(command.Event.ProviderId);

            if (provider == null) return;

            await SaveProvider(provider);
        }

        private Roatp.Api.Types.Provider GetProvider(string ukprn)
        {
            try
            {
                var provider = _roatpGateway.GetProvider(ukprn);

                if (provider == null)
                {
                    _logger.Info($"Roatp provider not returned for Ukprn: {ukprn}.  Therefore, nothing to be updated");
                }
                return provider;
            }
            catch (Exception ex)
            {
               _logger.Error(ex, $"Exception thrown getting Provider for Ukprn:{ukprn}, from Roatp Api Client");
                throw;
            }
        }

        private async Task SaveProvider(Roatp.Api.Types.Provider provider)
        {
            try
            {
                await _providerRepository.SaveProvider(provider);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception thrown saving Provider to RDS with Ukprn:{provider.Ukprn}");
                throw;
            }
        }
    }
}
