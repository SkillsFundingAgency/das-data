using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.EventsApiServiceTests
{
    [TestFixture]
    public class WhenIGetUnprocessedGenericEvents
    {
        private Mock<IEventsApi> _eventsApi;
        private Mock<IEventRepository> _eventsRepository;
        private EventsApiService _service;

        [SetUp]
        public void Arrange()
        {
            _eventsApi = new Mock<IEventsApi>();
            _eventsRepository = new Mock<IEventRepository>();

            _service = new EventsApiService(_eventsApi.Object, _eventsRepository.Object);
        }

        [Test]
        public async Task ThenTheEventsAreReturned()
        {
            var eventType = "Event Type";
            var lastEventId = 123;
            var expectedEvents = new List<GenericEvent>();

            _eventsRepository.Setup(x => x.GetLastProcessedEventId<long>(eventType)).ReturnsAsync(lastEventId);
            _eventsApi.Setup(x => x.GetGenericEventsById(eventType, lastEventId + 1, 1000, 1)).ReturnsAsync(expectedEvents);

            var response = await _service.GetUnprocessedGenericEvents(eventType);

            Assert.AreSame(expectedEvents, response);
        }
    }
}
