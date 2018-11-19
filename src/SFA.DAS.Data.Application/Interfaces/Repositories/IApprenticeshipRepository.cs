using System.Threading.Tasks;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IApprenticeshipRepository
    {
        Task Create(ApprenticeshipEventView @event);
        Task<long> GetTotalNumberOfAgreedApprenticeships();
    }
}
