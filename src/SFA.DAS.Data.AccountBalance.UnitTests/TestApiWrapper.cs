using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.AccountBalance.UnitTests
{
    [TestFixture]
    public class TestApiWrapper
    {
        public class FakeAccountApi : IAccountApiClient
        {
            private PagedApiResponseViewModel<AccountWithBalanceViewModel> res(int page)
            {
                return new PagedApiResponseViewModel<AccountWithBalanceViewModel>
                {
                    Data = Enumerable.Range(0, 10)
                        .Select(i => new AccountWithBalanceViewModel {AccountHashId = i.ToString()}).ToList(),
                    Page = 0,
                    TotalPages = 3 - page
                };
            }

            public Task<PagedApiResponseViewModel<AccountWithBalanceViewModel>> GetPageOfAccounts(int pageNumber = 1, int pageSize = 1000, DateTime? toDate = null)
            {
                return Task.Run(() => res(pageNumber));
            }
        }


        [Test]
        public void CanPage()
        {
            var fake = new FakeAccountApi();

            var wrapper = new ApiWrapper(fake);

            var reults = wrapper.GetAccounts().ToList();
        }
    }

    [TestFixture]
    public class IntegrationTest
    {
        [Ignore("jsut for integration")]
        public void CanCall()
        {
            var accountApiClientConfiguration = new AccountApiConfiguration
            {
                ApiBaseUrl = "https://at-accounts.apprenticeships.sfa.bis.gov.uk",
                ClientId = "58a3a7b2-bae2-4333-8a79-8f39720b2a6e",
                ClientSecret = @"7PrJ57WnGwHog6N7SanpXduDGdTrN/redY/22iKEulE=",
                IdentifierUri = "https://citizenazuresfabisgov.onmicrosoft.com/eas-api",
                Tenant = "citizenazuresfabisgov.onmicrosoft.com"
            };

            var client = new AccountApiClient(accountApiClientConfiguration);

            var page = client.GetPageOfAccounts().Result;

            Assert.NotNull(page);
        }
    }
}
