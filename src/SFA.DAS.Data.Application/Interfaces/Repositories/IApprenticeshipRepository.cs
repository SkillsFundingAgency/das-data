using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IApprenticeshipRepository
    {
        Task Create(ApprenticeshipEvent @event);
        Task<long> GetTotalNumberOfAgreedApprenticeships();
    }
}
