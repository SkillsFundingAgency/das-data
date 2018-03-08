using System.Configuration;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.PaymentEventTests
{
    public abstract class PaymentEventTestsBase : EventTestBase
    {
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.ProviderEventsApi;

        protected override void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeletePayments().Wait();
            EventTestsRepository.StoreLastProcessedEventId("PeriodEnd", "PERIOD2").Wait();
        }
    }
}
