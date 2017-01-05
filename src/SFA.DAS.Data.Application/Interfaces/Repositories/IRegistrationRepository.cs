using System.Threading.Tasks;
using SFA.DAS.Data.Application.Dtos;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IRegistrationRepository
    {
        Task SaveRegistration(RegistrationViewModel registration);
    }
}
