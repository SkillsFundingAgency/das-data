using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{ 
    [TestFixture]
    public class WhenIProcessEvents : EventProcessorTests
    {
        [Test]
        public async Task AndNoEventsAreReturned()
        {
            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(new List<AccountEventView>());

            await EventProcessor.ProcessEvents();

            EventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", It.IsAny<long>()), Times.Never);
            Logger.Verify(x => x.Info("No events to process."), Times.Once);
        }

        [Test]
        public async Task ThenTheRegistrationsAreCreated()
        {
            var lastEventId = 43908;
            var expectedEvents = new List<AccountEventView>
            {
                new AccountEventView {EmployerAccountId = "dsf895u", Id = lastEventId - 3},
                new AccountEventView {EmployerAccountId = "fvn3458t", Id = lastEventId - 2},
                new AccountEventView {EmployerAccountId = "cfdvklt4", Id = lastEventId - 1},
                new AccountEventView {EmployerAccountId = "cdvkj545", Id = lastEventId}
            };

            EventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(expectedEvents);

            await EventProcessor.ProcessEvents();

            foreach (var @event in expectedEvents)
            {
                Mediator.Verify(x => x.SendAsync(It.Is<CreateRegistrationCommand>(r => r.DasAccountId == @event.EmployerAccountId)), Times.Once);
                Logger.Verify(x => x.Info($"Event {@event.Id} processed"));
            }

            EventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", lastEventId), Times.Once);
        }
    }
}
