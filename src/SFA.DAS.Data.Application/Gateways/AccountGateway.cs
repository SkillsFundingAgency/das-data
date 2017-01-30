using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Gateways
{
    public class AccountGateway : IAccountGateway
    {
        private readonly IAccountApiClient _accountApiClient;

        public AccountGateway(IAccountApiClient accountApiClient)
        {
            _accountApiClient = accountApiClient;
        }

        public async Task<AccountDetailViewModel> GetAccount(string accountHref)
        {
            return await _accountApiClient.GetResource<AccountDetailViewModel>(accountHref);
        }
    }
}
