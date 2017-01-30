using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IAccountGateway
    {
        Task<AccountDetailViewModel> GetAccount(string accountHref);
    }
}
