using System.Threading.Tasks;
using SFA.DAS.Data.Application.Dtos;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IRegistrationGateway
    {
        Task<RegistrationViewModel> GetRegistration(int organisationId);
    }
}
