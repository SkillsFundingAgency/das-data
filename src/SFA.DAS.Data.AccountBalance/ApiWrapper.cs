using System.Collections.Generic;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.AccountBalance
{
    public class ApiWrapper
    {
        public IAccountApiClient Client { get; set; }

        public ApiWrapper(IAccountApiClient client)
        {
            Client = client;
        }

        public IEnumerable<AccountWithBalanceViewModel> GetAccounts()
        {
            var page = Client.GetPageOfAccounts().Result;

            foreach (var accountWithBalanceViewModel in page.Data)
                yield return accountWithBalanceViewModel;

            for (var p = 2; p <= page.TotalPages; p++)
            {
                page = Client.GetPageOfAccounts(p).Result;
                foreach (var accountWithBalanceViewModel in page.Data)
                    yield return accountWithBalanceViewModel;
            }
        }
    }
}
