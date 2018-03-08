using System.Configuration;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.ApprenticeshipEventTests
{
    public abstract class ApprenticeshipEventTestsBase : EventTestBase
    {
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.EventsApi;

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteApprenticeships().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId("ApprenticeshipEventView", 2).Wait();
        }
    }
}
