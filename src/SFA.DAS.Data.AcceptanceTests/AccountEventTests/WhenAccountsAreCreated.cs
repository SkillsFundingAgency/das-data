using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Types.Events;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenAccountsAreCreated : AccountEventTestsBase
    {
        protected override string EventName => "AccountCreatedEvent";

        [Test]
        public void ThenTheAccountDetailsAreStored()
        {
            ConfigureEventsApi();
            ConfigureAccountsApi();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId(EventName);
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            var numberOfRegistrations = await EventTestsRepository.GetNumberOfAccounts();
            if (numberOfRegistrations != 2)
            {
                return false;
            }

            var numberOfLegalEntities = await EventTestsRepository.GetNumberOfLegalEntities();
            if (numberOfLegalEntities != 3)
            {
                return false;
            }

            var numberOfPayeSchemes = await EventTestsRepository.GetNumberOfPayeSchemes();
            if (numberOfPayeSchemes != 3)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi()
        {
            AccountsApi.SetupGet("api/accounts/ABC123",
                new AccountDetailViewModelBuilder().WithDasAccountId("ABC123")
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/legalentities/123"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/payeschemes/1234"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/payeschemes/5678"))
                    .Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999",
                new AccountDetailViewModelBuilder().WithDasAccountId("ZZZ999")
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ZZZ999/legalentities/9876"))
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ZZZ999/legalentities/5432"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ZZZ999/payeschemes/9876"))
                    .Build());

            AccountsApi.SetupGet("api/accounts/ABC123/legalentities/123", new LegalEntityViewModelBuilder().WithDasAccountId("ABC123").WithLegalEntityId(123).Build());
            AccountsApi.SetupGet("api/accounts/ABC123/payeschemes/1234", new PayeSchemeViewModelBuilder().WithDasAccountId("ABC123").WithRef("1234").Build());
            AccountsApi.SetupGet("api/accounts/ABC123/payeschemes/5678", new PayeSchemeViewModelBuilder().WithDasAccountId("ABC123").WithRef("5678").Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999/legalentities/9876", new LegalEntityViewModelBuilder().WithDasAccountId("ZZZ999").WithLegalEntityId(9876).Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999/legalentities/5432", new LegalEntityViewModelBuilder().WithDasAccountId("ZZZ999").WithLegalEntityId(5432).Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999/payeschemes/9876", new PayeSchemeViewModelBuilder().WithDasAccountId("ZZZ999").WithRef("9876").Build());
        }

        private void ConfigureEventsApi()
        {
            var events = new List<GenericEvent>
            {
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    Type = "AccountCreatedEvent",
                    Payload = JsonConvert.SerializeObject(new AccountCreatedEvent
                    {
                        ResourceUri = "api/accounts/ABC123"
                    })
                }
                ,
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    Type = "AccountCreatedEvent",
                    Payload = JsonConvert.SerializeObject(new AccountCreatedEvent
                    {
                        ResourceUri = "api/accounts/ZZZ999"
                    })
                }
            };

            EventsApi.SetupGet($"api/events/getSinceEvent?eventType={EventName}&fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
