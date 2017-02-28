using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateAccount;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{ 
    [TestFixture]
    public class WhenIProcessEvents : EventProcessorTests
    {
        [Test]
        public async Task AndNoEventsAreReturnedThenALogEntryIsCreated()
        {
            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(new List<AccountEventView>());

            await EventsWatcher.ProcessEvents();

            EventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", It.IsAny<long>()), Times.Never);
            Logger.Verify(x => x.Info("No events to process."), Times.Once);
        }

        [Test]
        public async Task ThenTheRegistrationsAreCreated()
        {
            var lastEventId = 43908;
            var expectedEvents = new List<AccountEventView>
            {
                new AccountEventView {ResourceUri = "dsf895u", Id = lastEventId - 3},
                new AccountEventView {ResourceUri = "fvn3458t", Id = lastEventId - 2},
                new AccountEventView {ResourceUri = "cfdvklt4", Id = lastEventId - 1},
                new AccountEventView {ResourceUri = "cdvkj545", Id = lastEventId}
            };

            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(expectedEvents);

            await EventsWatcher.ProcessEvents();

            foreach (var @event in expectedEvents)
            {
                EventDispatcher.Verify(x => x.Dispatch(@event), Times.Once);
                Logger.Verify(x => x.Info($"Event {@event.Id} processed"));
            }

            EventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", lastEventId), Times.Once);
        }
    }
}
