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
}
