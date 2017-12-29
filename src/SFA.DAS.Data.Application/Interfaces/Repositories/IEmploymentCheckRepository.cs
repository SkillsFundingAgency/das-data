using System.Threading.Tasks;
using SFA.DAS.EmploymentCheck.Events;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IEmploymentCheckRepository
    {
        Task SaveEmploymentCheck(EmploymentCheckCompleteEvent employmentCheck);
    }
}
