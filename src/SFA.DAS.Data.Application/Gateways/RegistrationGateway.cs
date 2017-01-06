using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Application.Gateways
{
    public class RegistrationGateway : IRegistrationGateway
    {
        private readonly IAccountApiClient _accountApiClient;

        public RegistrationGateway(IAccountApiClient accountApiClient)
        {
            _accountApiClient = accountApiClient;
        }

        public async Task<IEnumerable<AccountInformationViewModel>> GetRegistration(string dasAccountId)
        {
            return await GetRegistrations(dasAccountId, 1);
        }

        private async Task<IEnumerable<AccountInformationViewModel>> GetRegistrations(string dasAccountId, int page)
        {
            var accountInformation = await _accountApiClient.GetPageOfAccountInformation(DateTime.MinValue, DateTime.MaxValue, page);
            var registrationsForAccount = accountInformation.Data.Where(i => i.DasAccountId == dasAccountId).ToList();

            if (MatchedAccountIsLastResult(accountInformation, registrationsForAccount) && ResultContainsMorePages(page, accountInformation))
            {
                var nextPageOfRegistrations = await GetRegistrations(dasAccountId, page + 1);
                registrationsForAccount.AddRange(nextPageOfRegistrations);
            }
            return registrationsForAccount;
        }

        private static bool ResultContainsMorePages(int page, PagedApiResponseViewModel<AccountInformationViewModel> accountInformation)
        {
            return accountInformation.TotalPages > page;
        }

        private static bool MatchedAccountIsLastResult(PagedApiResponseViewModel<AccountInformationViewModel> accountInformation, List<AccountInformationViewModel> registrationsForAccount)
        {
            return accountInformation.Data.IndexOf(registrationsForAccount.Last()) == accountInformation.Data.Count - 1;
        }
    }
}
