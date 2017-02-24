using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface ICommitmentApprenticeshipRepository
    {
        Task Create(CommitmentsApprenticeshipEvent @event);
    }
}
