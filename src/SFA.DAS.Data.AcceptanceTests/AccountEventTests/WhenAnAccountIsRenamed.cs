using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenAnAccountIsRenamed : AccountEventTestsBase
    {
        [Test]
        public void ThenTheAccountDetailsAreStored()
        {
            var events = ConfigureEventsApi();
            ConfigureAccountsApi(events);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId("AccountEvents");
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            var numberOfAccounts = await EventTestsRepository.GetNumberOfAccounts();
            if (numberOfAccounts != 2)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi(List<AccountEventView> events)
        {
            AccountsApi.SetupGet("api/accounts/ABC123",
                new AccountDetailViewModelBuilder().WithDasAccountId("ABC123")
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/legalentities/123"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/payeschemes/1234"))
                    .Build());

            AccountsApi.SetupGet("api/accounts/ABC123/legalentities/123", new LegalEntityViewModelBuilder().WithDasAccountId("ABC123").WithLegalEntityId(123).Build());
            AccountsApi.SetupGet("api/accounts/ABC123/payeschemes/1234", new PayeSchemeViewModelBuilder().WithDasAccountId("ABC123").WithRef("1234").Build());

            AccountsApi.SetupGet("api/accounts/ABC123v2", new AccountDetailViewModelBuilder().WithDasAccountId("ABC123").WithName("New Name").Build());
        }

        private List<AccountEventView> ConfigureEventsApi()
        {
            var events = new List<AccountEventView>
            {
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    ResourceUri = "api/accounts/ABC123",
                    Event = "AccountCreated"
                },
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    ResourceUri = "api/accounts/ABC123v2",
                    Event = "AccountRenamed"
                }
            };

            EventsApi.SetupGet("api/events/accounts?fromEventId=3&pageSize=1000&pageNumber=1", events);
            return events;
        }
    }
}
