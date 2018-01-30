using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IProviderRepository
    {
        Task SaveProvider(Roatp.Api.Types.Provider provider);
    }
}
