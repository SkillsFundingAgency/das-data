using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;

namespace SFA.DAS.Data.AcceptanceTests.DataLockEventTests
{
    public abstract class DataLockEventTestsBase : EventTestBase
    {
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.ProviderEventsApi;

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteDataLocks().Wait();
            EventTestsRepository.StoreLastProcessedEventId("DataLockEvent", "0").Wait();
        }
    }
}
