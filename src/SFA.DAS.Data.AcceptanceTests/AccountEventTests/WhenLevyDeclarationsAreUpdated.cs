﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EAS.Account.Api.Types.Events.Levy;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenLevyDeclarationsAreUpdated : AccountEventTestsBase
    {
        protected override string EventName => "LevyDeclarationUpdatedEvent";

        [Test]
        public void ThenTheLevyDeclarationsAreStored()
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
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<long>(EventName);
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            var numberOfLevyDeclarations = await EventTestsRepository.GetNumberOfLevyDeclarations();
            if (numberOfLevyDeclarations != 3)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi()
        {
            AccountsApi.SetupGet("api/accounts/ABC123/levy", new AccountResourceList<LevyDeclarationViewModel>(new[] { new LevyDeclarationViewModelBuilder().WithDasAccountId("ABC123").Build() }));
            AccountsApi.SetupGet("api/accounts/ZZZ999/levy",
                new AccountResourceList<LevyDeclarationViewModel>(
                new[] {
                    new LevyDeclarationViewModelBuilder().WithDasAccountId("ZZZ999").Build(),
                    new LevyDeclarationViewModelBuilder().WithDasAccountId("ZZZ999").Build()
                }));
        }

        private void ConfigureEventsApi()
        {
            var events = new List<GenericEvent>
            {
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    Type = "LevyDeclarationUpdatedEvent",
                    Payload = JsonConvert.SerializeObject(new LevyDeclarationUpdatedEvent
                    {
                        ResourceUri = "api/accounts/ABC123/levy"

                    })
                },
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    Type = "LevyDeclarationUpdatedEvent",
                    Payload = JsonConvert.SerializeObject(new LevyDeclarationUpdatedEvent
                    {
                        ResourceUri = "api/accounts/ZZZ999/levy"
                    })
                }
            };

            EventsApi.SetupGet($"api/events/getSinceEvent?eventType={EventName}&fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
