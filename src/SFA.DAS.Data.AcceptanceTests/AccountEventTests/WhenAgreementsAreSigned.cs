using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.EAS.Account.Api.Types.Events.Agreement;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenAgreementsAreSigned : AccountEventTestsBase
    {
        protected override string EventName => "AgreementSignedEvent";

        [Test]
        public void ThenTheAgreementsAreStored()
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

            var numberOfAgreements = await EventTestsRepository.GetNumberOfEmployerAgreements();
            if (numberOfAgreements != 2)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi()
        {
            AccountsApi.SetupGet("api/accounts/ABC123/declarations/321CBA", new EmployerAgreementViewBuilder().WithDasAccountId("ABC123").WithDasAgreementId("321CBA").Build());
            AccountsApi.SetupGet("api/accounts/ZZZ999/declarations/999ZZZ", new EmployerAgreementViewBuilder().WithDasAccountId("ZZZ999").WithDasAgreementId("999ZZZ").Build());
        }

        private void ConfigureEventsApi()
        {
            var events = new List<GenericEvent>
            {
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    Type = "AgreementSignedEvent",
                    Payload = JsonConvert.SerializeObject(new AgreementSignedEvent
                    {
                        ResourceUrl = "api/accounts/ABC123/declarations/321CBA"

                    })
                },
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    Type = "AgreementSignedEvent",
                    Payload = JsonConvert.SerializeObject(new AgreementSignedEvent
                    {
                        ResourceUrl = "api/accounts/ZZZ999/declarations/999ZZZ"
                    })
                }
            };

            EventsApi.SetupGet($"api/events/getSinceEvent?eventType={EventName}&fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
