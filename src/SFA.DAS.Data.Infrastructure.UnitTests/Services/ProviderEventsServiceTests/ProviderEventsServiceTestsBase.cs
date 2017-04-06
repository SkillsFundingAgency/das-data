using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.Provider.Events.Api.Client;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.ProviderEventsServiceTests
{
    public class ProviderEventsServiceTestsBase
    {
        protected Mock<IPaymentsEventsApiClient> EventsApi;
        protected Mock<IEventRepository> EventsRepository;
        protected ProviderEventsService Service;

        [SetUp]
        public void Arrange()
        {
            EventsApi = new Mock<IPaymentsEventsApiClient>();
            EventsRepository = new Mock<IEventRepository>();

            Service = new ProviderEventsService(EventsApi.Object, EventsRepository.Object);
        }
    }
}
