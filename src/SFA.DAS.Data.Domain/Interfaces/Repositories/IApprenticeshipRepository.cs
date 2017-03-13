using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Domain.Interfaces.Repositories
{
    public interface IApprenticeshipRepository
    {
        Task Create(CommitmentsApprenticeshipEvent @event);
    }
}
