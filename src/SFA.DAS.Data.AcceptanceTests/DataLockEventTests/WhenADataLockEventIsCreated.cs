﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.DataLockEventTests
{
    [TestFixture]
    public class WhenADataLockEventIsCreated : DataLockEventTestsBase
    {
        protected override string EventName => "DataLockEvent";

        [Test]
        public void ThenTheDataLockEventsAreStored()
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

            return true;
        }

        private void ConfigureEventsApi()
        {
            ConfigureDataLockEvents(0, 3);
        }
        
        private void ConfigureDataLockEvents(int sinceEventid, int numberOfDataLockEvents)
        {
            var dataLockEvents = new List<DataLockEvent>();
            for (var i = 1; i <= numberOfDataLockEvents; i++)
            {
                dataLockEvents.Add(new DataLockEventBuilder().WithId(i).Build());
            }
            var dataLockEventsResult = new PageOfResults<DataLockEvent> { Items = dataLockEvents.ToArray(), PageNumber = 1, TotalNumberOfPages = 1 };
            
            EventsApi.SetupGet($"api/datalock?page=1&sinceEventId={sinceEventid}&employerAccountId={null}&ukprn={null}", dataLockEventsResult);
        }
    }
}
