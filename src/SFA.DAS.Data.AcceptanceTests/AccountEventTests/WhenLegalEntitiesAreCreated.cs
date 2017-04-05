using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Types.Events.LegalEntity;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenLegalEntitiesAreCreated : AccountEventTestsBase
    {
        protected override string EventName => "LegalEntityCreatedEvent";

        [Test]
        public void ThenTheLegalEntityDetailsAreStored()
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

            var numberOfLegalEntities = await EventTestsRepository.GetNumberOfLegalEntities();
            if (numberOfLegalEntities != 2)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi()
        {
            AccountsApi.SetupGet("api/accounts/ABC123/legalentities/123", new LegalEntityViewModelBuilder().WithDasAccountId("ABC123").WithLegalEntityId(123).Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999/legalentities/9876", new LegalEntityViewModelBuilder().WithDasAccountId("ZZZ999").WithLegalEntityId(9876).Build());
        }

        private void ConfigureEventsApi()
        {
            var events = new List<GenericEvent>
            {
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    Type = "LegalEntityCreatedEvent",
                    Payload = JsonConvert.SerializeObject(new LegalEntityCreatedEvent
                    {
                        ResourceUri = "api/accounts/ABC123/legalentities/123"
                    })
                },
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    Type = "LegalEntityCreatedEvent",
                    Payload = JsonConvert.SerializeObject(new LegalEntityCreatedEvent
                    {
                        ResourceUri = "api/accounts/ZZZ999/legalentities/9876"
                    })
                }
            };

            EventsApi.SetupGet($"api/events/getSinceEvent?eventType={EventName}&fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
