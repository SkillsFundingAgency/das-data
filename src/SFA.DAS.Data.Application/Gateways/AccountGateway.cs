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

        public async Task<LegalEntityViewModel> GetLegalEntity(string legalEntityHref)
        {
            return await _accountApiClient.GetResource<LegalEntityViewModel>(legalEntityHref);
        }

        public async Task<PayeSchemeViewModel> GetPayeScheme(string payeSchemeHref)
        {
            return await _accountApiClient.GetResource<PayeSchemeViewModel>(payeSchemeHref);
        }
    }
}
