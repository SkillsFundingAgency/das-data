using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{ 
    [TestFixture]
    public class EventProcessorTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IEventRepository> _eventRepository;
        private Mock<IEventsApi> _eventsApi;
        private EventProcessor _eventProcessor;

        private const long CurrentEventId = 2345;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _eventRepository = new Mock<IEventRepository>();
            _eventsApi = new Mock<IEventsApi>();

            _eventRepository.Setup(x => x.GetLastProcessedEventId("AccountEvents")).ReturnsAsync(CurrentEventId);

            _eventProcessor = new EventProcessor(_eventRepository.Object, _eventsApi.Object, _mediator.Object);
        }

        [Test]
        public async Task AndNoEventsAreReturned()
        {
            _eventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(new List<AccountEventView>());

            await _eventProcessor.ProcessEvents();

            _eventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", It.IsAny<long>()), Times.Never);
        }

        [Test]
        public async Task ThenTheRegistrationsAreCreated()
        {
            var lastEventId = 43908;
            var expectedEvents = new List<AccountEventView>
            {
                new AccountEventView {EmployerAccountId = "dsf895u"},
                new AccountEventView {EmployerAccountId = "fvn3458t"},
                new AccountEventView {EmployerAccountId = "cfdvklt4"},
                new AccountEventView {EmployerAccountId = "cdvkj545", Id = lastEventId}
            };

            _eventsApi.Setup(x => x.GetAccountEventsById(CurrentEventId + 1, 1000, 1)).ReturnsAsync(expectedEvents);

            await _eventProcessor.ProcessEvents();

            foreach (var @event in expectedEvents)
            {
                _mediator.Verify(x => x.SendAsync(It.Is<CreateRegistrationCommand>(r => r.DasAccountId == @event.EmployerAccountId)), Times.Once);
            }

            _eventRepository.Verify(x => x.StoreLastProcessedEventId("AccountEvents", lastEventId), Times.Once);
        }
    }
}
