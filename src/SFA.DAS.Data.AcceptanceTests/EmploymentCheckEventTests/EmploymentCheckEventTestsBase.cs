using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.EmploymentCheckEventTests
{
    public abstract class EmploymentCheckEventTestsBase : EventTestBase
    {
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.EventsApi;

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteEmploymentChecks().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId("EmploymentCheckCompleteEvent", 2).Wait();
        }
    }
}
