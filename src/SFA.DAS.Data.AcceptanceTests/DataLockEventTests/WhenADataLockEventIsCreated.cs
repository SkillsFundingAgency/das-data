using System;
using System.Collections.Generic;
using System.Linq;
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
        [Test]
        public async Task ThenTheDataLockEventsAreStored()
        {
            var expectedDataLockEvents = ConfigureEventsApi();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);

            await AreDataLockFieldsStoredCorrectly(expectedDataLockEvents);
        }

        private async Task AreDataLockFieldsStoredCorrectly(IList<DataLockEvent> expectedDataLockEvents)
        {
            var actualDataLocks = (await EventTestsRepository.GetDataLocks()).OrderBy(d => d.DataLockEventId).ToList();
            var actualDataLockErrors = (await EventTestsRepository.GetDataLockErrors()).OrderBy(e => e.DataLockId).ThenBy(e => e.ErrorCode).ToList();

            for (var i = 0; i < actualDataLocks.Count; i++)
            {
                Assert.AreEqual(expectedDataLockEvents[i].Id, actualDataLocks[i].DataLockEventId);

                var expectedDataLock = expectedDataLockEvents[i];
                var actualDataLock = actualDataLocks[i];

                Assert.AreEqual(expectedDataLock.ProcessDateTime, actualDataLock.ProcessDateTime);
                Assert.AreEqual(expectedDataLock.Status, actualDataLock.Status);
                Assert.AreEqual(expectedDataLock.IlrFileName, actualDataLock.IlrFileName);
                Assert.AreEqual(expectedDataLock.Ukprn, actualDataLock.UkPrn);
                Assert.AreEqual(expectedDataLock.Uln, actualDataLock.Uln);
                Assert.AreEqual(expectedDataLock.LearnRefNumber, actualDataLock.LearnRefNumber);
                Assert.AreEqual(expectedDataLock.AimSeqNumber, actualDataLock.AimSeqNumber);
                Assert.AreEqual(expectedDataLock.PriceEpisodeIdentifier, actualDataLock.PriceEpisodeIdentifier);
                Assert.AreEqual(expectedDataLock.ApprenticeshipId, actualDataLock.ApprenticeshipId);
                Assert.AreEqual(expectedDataLock.EmployerAccountId, actualDataLock.EmployerAccountId);
                Assert.AreEqual(expectedDataLock.EventSource, actualDataLock.EventSource);
                Assert.AreEqual(expectedDataLock.IlrStartDate, actualDataLock.IlrStartDate);
                Assert.AreEqual(expectedDataLock.IlrStandardCode, actualDataLock.IlrStandardCode);
                Assert.AreEqual(expectedDataLock.IlrProgrammeType, actualDataLock.IlrProgrammeType);
                Assert.AreEqual(expectedDataLock.IlrFrameworkCode, actualDataLock.IlrFrameworkCode);
                Assert.AreEqual(expectedDataLock.IlrPathwayCode, actualDataLock.IlrPathwayCode);
                Assert.AreEqual(expectedDataLock.IlrTrainingPrice, actualDataLock.IlrTrainingPrice);
                Assert.AreEqual(expectedDataLock.IlrEndpointAssessorPrice, actualDataLock.IlrEndpointAssessorPrice);
                Assert.AreEqual(expectedDataLock.IlrPriceEffectiveFromDate, actualDataLock.IlrPriceEffectiveFromDate);
                Assert.AreEqual(expectedDataLock.IlrPriceEffectiveToDate, actualDataLock.IlrPriceEffectiveToDate);
                Assert.AreEqual(true, actualDataLock.IsLatest);

                //Check errors
                var actualErrors = actualDataLockErrors.Where(e => e.DataLockId == actualDataLock.Id).OrderBy(e => e.ErrorCode)
                    .ToList();
                Assert.AreEqual(expectedDataLock.Errors.Length, actualErrors.Count);
                foreach (var expectedError in expectedDataLock.Errors)
                {
                    var actualError = actualErrors.SingleOrDefault(e => e.ErrorCode == expectedError.ErrorCode);
                    Assert.IsNotNull(actualError);
                    Assert.AreEqual(expectedError.SystemDescription, actualError.SystemDescription);
                }
            }
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<string>(EventName);
            if (lastProcessedEventId != "1")
            {
                return false;
            }

            var numberOfDataLockEvents = await EventTestsRepository.GetNumberOfDataLockEvents();
            if (numberOfDataLockEvents != 1)
            {
                return false;
            }

            var numberOfDataLockErrors = await EventTestsRepository.GetNumberOfDataLockErrors();
            if (numberOfDataLockErrors != 1)
            {
                return false;
            }

            return true;
        }

        private IList<DataLockEvent> ConfigureEventsApi()
        {
            return ConfigureDataLockEvents(0, 1);
        }

        private IList<DataLockEvent> ConfigureDataLockEvents(int sinceEventId, int numberOfDataLockEvents)
        {
            var dataLockEvents = new List<DataLockEvent>();
            for (var i = 1; i <= numberOfDataLockEvents; i++)
            {
                dataLockEvents.Add(new DataLockEventBuilder().WithId(i).Build());
            }

            var dataLockEventsResult = new PageOfResults<DataLockEvent>
            { Items = dataLockEvents.ToArray(), PageNumber = 1, TotalNumberOfPages = 1 };

            EventsApi.SetupGet($"api/datalock?page=1", dataLockEventsResult);
            EventsApi.SetupGet($"api/datalock?page=1&sinceEventId={sinceEventId}", dataLockEventsResult);

            return dataLockEvents;
        }
    }
}
