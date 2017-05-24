using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IRoatpGateway
    {
        Task<Roatp.Api.Types.Provider> GetProvider(long providerId);
    }
}
