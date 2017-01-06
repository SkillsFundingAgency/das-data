using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IRegistrationGateway
    {
        Task<IEnumerable<AccountInformationViewModel>> GetRegistration(string dasAccountId);
    }
}
