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
    public class WhenAPayeSchemeIsAddedToAnAccount : AccountEventTestsBase
    {
        protected override string EventName => "AccountEventView";

        [Test]
        public void ThenThePayeSchemeDetailsAreStored()
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
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId("AccountEventView");
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            var numberOfLegalEntities = await EventTestsRepository.GetNumberOfPayeSchemes();
            if (numberOfLegalEntities != 2)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi(List<AccountEventView> events)
        {
            AccountsApi.SetupGet("api/accounts/ABC123/payeschemes/123", new PayeSchemeViewModelBuilder().WithDasAccountId("ABC123").WithRef("123").Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999/payeschemes/9876", new PayeSchemeViewModelBuilder().WithDasAccountId("ZZZ999").WithRef("9876").Build());
        }

        private List<AccountEventView> ConfigureEventsApi()
        {
            var events = new List<AccountEventView>
            {
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    ResourceUri = "api/accounts/ABC123/payeschemes/123",
                    Event = "PayeSchemeAdded"
                },
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    ResourceUri = "api/accounts/ZZZ999/payeschemes/9876",
                    Event = "PayeSchemeAdded"
                }
            };

            EventsApi.SetupGet("api/events/accounts?fromEventId=3&pageSize=1000&pageNumber=1", events);
            return events;
        }
    }
}
