using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.Roatp.Api.Client;

namespace SFA.DAS.Data.Application.Gateways
{
    public class RoatpGateway : IRoatpGateway
    {
        private IRoatpClient _roatpClient;

        public RoatpGateway(IRoatpClient roatpClient)
        {
            _roatpClient = roatpClient;
        }

        public async Task<Roatp.Api.Types.Provider> GetProvider(long providerId)
        {
            var provider = _roatpClient.Get(providerId);
            return provider;
        }
    }
}
