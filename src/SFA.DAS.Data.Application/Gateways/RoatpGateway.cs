using System;
using System.Collections.Generic;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.Roatp.Api.Client;

namespace SFA.DAS.Data.Application.Gateways
{
    public class RoatpGateway : IRoatpGateway
    {
        private readonly IRoatpClient _roatpClient;

        public RoatpGateway(IRoatpClient roatpClient)
        {
            _roatpClient = roatpClient ?? throw new ArgumentNullException(nameof(roatpClient));
        }

        public Roatp.Api.Types.Provider GetProvider(string ukPrn)
        {
            return _roatpClient.Get(ukPrn);
        }

        public bool ProviderExists(string ukPrn)
        {
            return _roatpClient.Exists(ukPrn);
        }

        public IEnumerable<Roatp.Api.Types.Provider> FindAllProviders()
        {
            return _roatpClient.FindAll();
        }
    }
}
