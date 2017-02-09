﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenAccountsAreCreated : AccountEventTestsBase
    {
        [Test]
        public void ThenTheAccountDetailsAreStored()
        {
            Trace.WriteLine("ThenTheAccountDetailsAreStored started");

            Trace.WriteLine("Configuring APIs");
            var events = ConfigureEventsApi();
            ConfigureAccountsApi(events);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Trace.WriteLine("Running worker");
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            Trace.WriteLine("Checking database {0}", DateTime.Now.ToLongTimeString());
            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));
            Trace.WriteLine("Finished checking database {0}", DateTime.Now.ToLongTimeString());

            Trace.WriteLine("Cancelling token", DateTime.Now.ToLongTimeString());
            cancellationTokenSource.Cancel();
            Trace.WriteLine("Token cancelled", DateTime.Now.ToLongTimeString());
            Assert.IsTrue(databaseAsExpected);

            Trace.WriteLine("ThenTheAccountDetailsAreStored completed");
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            Trace.WriteLine("Getting last processed event id");
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId("AccountEvents");
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            Trace.WriteLine("Getting number of registrations");
            var numberOfRegistrations = await EventTestsRepository.GetNumberOfAccounts();
            if (numberOfRegistrations != 2)
            {
                return false;
            }

            Trace.WriteLine("Getting number of legal entities");
            var numberOfLegalEntities = await EventTestsRepository.GetNumberOfLegalEntities();
            if (numberOfLegalEntities != 3)
            {
                return false;
            }

            Trace.WriteLine("Getting number of paye schemes");
            var numberOfPayeSchemes = await EventTestsRepository.GetNumberOfPayeSchemes();
            if (numberOfPayeSchemes != 3)
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
                    ResourceUri = "api/accounts/ZZZ999",
                    Event = "AccountCreated"
                }
            };

            EventsApi.SetupGet("api/events/accounts?fromEventId=3&pageSize=1000&pageNumber=1", events);
            return events;
        }
    }
}
