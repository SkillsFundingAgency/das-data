using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types.Events.Agreement;
using SFA.DAS.EmploymentCheck.Events;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.EmploymentCheckEventTests
{
    [TestFixture]
    public abstract class WhenAnEmploymentCheckIsComplete : EmploymentCheckEventTestsBase
    {
        [Test]
        public void ThenTheApprenticeshipDetailsAreStored()
        {
            ConfigureEventsApi();
            
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<long>("EmploymentCheckCompleteEvent");
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            var numberOfEmploymentChecks = await EventTestsRepository.GetNumberOfEmploymentChecks();
            if (numberOfEmploymentChecks != 2)
            {
                return false;
            }

            return true;
        }

        private void ConfigureEventsApi()
        {
            var events = new List<GenericEvent>
            {
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    Type = "EmploymentCheckCompleteEvent",
                    Payload = JsonConvert.SerializeObject(new EmploymentCheckCompleteEvent("AB123456C", 123445, 434533, 3245346, DateTime.Now.AddDays(-20), true))
                },
                new GenericEvent
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    Type = "EmploymentCheckCompleteEvent",
                    Payload = JsonConvert.SerializeObject(new EmploymentCheckCompleteEvent("JA987654C", 94375, 234645, 3456843, DateTime.Now.AddYears(-1), false))
                }
            };

            EventsApi.SetupGet($"api/events/getSinceEvent?eventType=EmploymentCheckCompleteEvent&fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
