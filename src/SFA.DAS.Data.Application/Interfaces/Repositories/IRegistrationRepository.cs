using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IRegistrationRepository
    {
        Task SaveRegistration(AccountInformationViewModel registration);
    }
}
