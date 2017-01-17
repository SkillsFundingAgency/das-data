using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{
    [TestFixture]
    public class WhenProcessingAnEventFails : EventProcessorTests
    {
        [Test]
        public async Task ThenTheExceptionIsLoggedAndTheLastProcessedEventIsUpdated()
        {
            long failedEventId = 43908;
            var failedDasAccountId = "cfdvklt4";
            var expectedEvents = new List<AccountEventView>
            {
                new AccountEventView {EmployerAccountId = "dsf895u", Id = failedEventId - 2},
                new AccountEventView {EmployerAccountId = "fvn3458t", Id = failedEventId - 1},
                new AccountEventView {EmployerAccountId = failedDasAccountId, Id = failedEventId},
                new AccountEventView {EmployerAccountId = "cdvkj545", Id = failedEventId + 1}
            };

            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(expectedEvents);
            var expectedException = new Exception();
            Mediator.Setup(x => x.PublishAsync(It.Is<CreateRegistrationCommand>(c => c.DasAccountId == failedDasAccountId))).Throws(expectedException);
            EventRepository.Setup(x => x.GetEventFailureCount(failedEventId)).ReturnsAsync(FailureTolerance - 2);

            await EventProcessor.ProcessEvents();

            EventRepository.Verify(x => x.SetEventFailureCount(failedEventId, FailureTolerance - 1), Times.Once);
            EventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", failedEventId - 1), Times.Once);
            Logger.Verify(x => x.Error(expectedException, $"Unexcepted exception when processing event {failedEventId} from event stream AccountEvents."));
        }

        [Test]
        public async Task AndTheEventHasExceededTheFailureThreshold()
        {
            long failedEventId = 43908;
            var failedDasAccountId = "cfdvklt4";
            var expectedEvents = new List<AccountEventView>
            {
                new AccountEventView {EmployerAccountId = failedDasAccountId, Id = failedEventId},
            };

            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(expectedEvents);
            var expectedException = new Exception();
            Mediator.Setup(x => x.PublishAsync(It.Is<CreateRegistrationCommand>(c => c.DasAccountId == failedDasAccountId))).Throws(expectedException);

            EventRepository.Setup(x => x.GetEventFailureCount(failedEventId)).ReturnsAsync(FailureTolerance - 1);

            await EventProcessor.ProcessEvents();

            EventRepository.Verify(x => x.SetEventFailureCount(failedEventId, FailureTolerance), Times.Once);
            EventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", failedEventId), Times.Once);
            Logger.Verify(x => x.Info($"Event {failedEventId} from event stream AccountEvents has reached the fault tolerance and will no longer be retried."));
            Logger.Verify(x => x.Error(expectedException, $"Unexcepted exception when processing event {failedEventId} from event stream AccountEvents."));
        }
    }
}
