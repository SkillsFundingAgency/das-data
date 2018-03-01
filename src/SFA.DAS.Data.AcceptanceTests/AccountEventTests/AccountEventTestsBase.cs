using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    public abstract class AccountEventTestsBase : EventTestBase
    {
        protected WebApiSubstitute AccountsApi => DataAcceptanceTests.AccountsApi;
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.EventsApi;

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteAccounts().Wait();
            EventTestsRepository.DeleteLevyDeclarations().Wait();
            EventTestsRepository.DeleteEmployerAgreements().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId(EventName, 2).Wait();
        }
    }
}
