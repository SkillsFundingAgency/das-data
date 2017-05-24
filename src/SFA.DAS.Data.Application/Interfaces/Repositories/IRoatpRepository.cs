using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IRoatpRepository
    {
        Task SaveRoatpProvider(Roatp.Api.Types.Provider provider);
    }
}
