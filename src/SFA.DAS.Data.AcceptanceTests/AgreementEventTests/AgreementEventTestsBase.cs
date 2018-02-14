using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.AgreementEventTests
{
    public abstract class AgreementEventTestsBase : EventTestBase
    {
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.EventsApi;
        protected WebApiSubstitute AgreementsApi => DataAcceptanceTests.AgreementEventsApi;

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteProviders().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId(EventName, 2).Wait();
        }
    }
}
