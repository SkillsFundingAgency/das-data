using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Worker.UnitTests.EventProcessorTests
{
    public abstract class EventProcessorTests
    {
        protected Mock<IMediator> Mediator;
        protected Mock<IEventRepository> EventRepository;
        protected Mock<IEventsApi> EventsApi;
        protected Mock<ILog> Logger;
        protected EventProcessor EventProcessor;

        protected const long CurrentEventId = 2345;
        protected const int FailureTolerance = 3;

        [SetUp]
        public void Arrange()
        {
            Mediator = new Mock<IMediator>();
            EventRepository = new Mock<IEventRepository>();
            EventsApi = new Mock<IEventsApi>();
            Logger = new Mock<ILog>();

            EventRepository.Setup(x => x.GetLastProcessedEventId("AccountEvents")).ReturnsAsync(CurrentEventId);

            EventProcessor = new EventProcessor(EventRepository.Object, EventsApi.Object, Mediator.Object, Logger.Object, FailureTolerance);
        }
    }
}
