using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.DataLockEventTests
{
    [TestFixture]
    public class WhenADataLockEventWithNoErrorsIsCreated : DataLockEventTestsBase
    {
        [Test]
        public void ThenTheDataLockEventsWithNoErrorsAreStored()
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
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<string>(EventName);
            if (lastProcessedEventId != "3")
            {
                return false;
            }

            var numberOfDataLockEvents = await EventTestsRepository.GetNumberOfDataLockEvents();
            if (numberOfDataLockEvents != 3)
            {
                return false;
            }

            var numberOfDataLockApprenticeships = await EventTestsRepository.GetNumberOfDataLockApprenticeships();
            if (numberOfDataLockApprenticeships != 3)
            {
                return false;
            }

            var numberOfDataLockErrors = await EventTestsRepository.GetNumberOfDataLockErrors();
            if (numberOfDataLockErrors != 0)
            {
                return false;
            }

            var numberOfDataLockPeriods = await EventTestsRepository.GetNumberOfDataLockPeriods();
            if (numberOfDataLockPeriods != 3)
            {
                return false;
            }

            return true;
        }

        private void ConfigureEventsApi()
        {
            ConfigureDataLockEvents(0, 3);
        }

        private void ConfigureDataLockEvents(int sinceEventId, int numberOfDataLockEvents)
        {
            var dataLockEvents = new List<DataLockEvent>();
            for (var i = 1; i <= numberOfDataLockEvents; i++)
            {
                dataLockEvents.Add(new DataLockEventBuilder().WithId(i).WithNoErrors().Build());
            }
            var dataLockEventsResult = new PageOfResults<DataLockEvent> { Items = dataLockEvents.ToArray(), PageNumber = 1, TotalNumberOfPages = 1 };

            EventsApi.SetupGet($"api/datalock?page=1", dataLockEventsResult);
            EventsApi.SetupGet($"api/datalock?page=1&inceEventId={sinceEventId}", dataLockEventsResult);
        }
    }
}
