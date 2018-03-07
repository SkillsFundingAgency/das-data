using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AgreementEventTests
{
    [TestFixture]
    public class WhenANewProviderAgreementIsCreated : AgreementEventTestsBase
    {
        protected override string EventName => "AgreementEventView";

        // Test commented out due to backing out of Roatp provider changes as not been tested
        //[Test]
        //public void ThenTheProviderDetailsAreStored()
        //{
        //    ConfigureEventsApi();
        //    ConfigureAgreementsApi();

        //    var cancellationTokenSource = new CancellationTokenSource();
        //    var cancellationToken = cancellationTokenSource.Token;
        //    Task.Run(() => WorkerRole.Run(), cancellationToken);

        //    var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

        //    cancellationTokenSource.Cancel();
        //    Assert.IsTrue(databaseAsExpected);
        //}

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<long>(EventName);
            if (lastProcessedEventId != 3)
            {
                return false;
            }

            var numberOfProviders = await EventTestsRepository.GetNumberOfProviders();
            if (numberOfProviders != 1)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAgreementsApi()
        {
            var provider = new ProviderBuilder().Build();

            EventsApi.SetupGet("api/providers/12345678", provider);
        }

        private void ConfigureEventsApi()
        {
            var events = new List<AgreementEventView>()
            {
                new AgreementEventView()
                {
                    Id = 3,
                    Event = "INITIATED",
                    CreatedOn = DateTime.Now.AddDays(-1),
                    ProviderId = "12345678",
                    ContractType = "ProviderAgreement"
                }
            };

            AgreementsApi.SetupGet("api/events/engagements?fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
