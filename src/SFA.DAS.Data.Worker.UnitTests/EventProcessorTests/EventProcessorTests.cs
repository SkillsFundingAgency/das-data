using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Worker.Events;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Dispatcher;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{
    public abstract class EventProcessorTests
    {
        protected Mock<IEventDispatcher> EventDispatcher;
        protected Mock<IEventRepository> EventRepository;
        protected Mock<IEventsApi> EventsApi;
        protected Mock<ILog> Logger;
        protected EventsWatcher EventsWatcher;

        protected const long CurrentEventId = 2345;
        protected const int FailureTolerance = 3;

        [SetUp]
        public void Arrange()
        {
            EventDispatcher = new Mock<IEventDispatcher>();
            EventRepository = new Mock<IEventRepository>();
            EventsApi = new Mock<IEventsApi>();
            Logger = new Mock<ILog>();

            EventRepository.Setup(x => x.GetLastProcessedEventId("AccountEvents")).ReturnsAsync(CurrentEventId);

            EventsWatcher = new EventsWatcher(EventRepository.Object, EventsApi.Object, EventDispatcher.Object, Logger.Object, FailureTolerance);
        }
    }
}
