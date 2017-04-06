using System.Configuration;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.PaymentEventTests
{
    public abstract class PaymentEventTestsBase
    {
        protected WorkerRole WorkerRole;
        protected EventTestsRepository EventTestsRepository;

        protected WebApiSubstitute EventsApi => DataAcceptanceTests.ProviderEventsApi;

        [SetUp]
        public void Arrange()
        {
            ClearSubstituteApis();
            StartWorkerRole();
            SetupDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            Task.Run(() => WorkerRole?.OnStop());
            WorkerRole = null;
        }

        private void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(ConfigurationManager.AppSettings["DataConnectionString"]);
            EventTestsRepository.DeletePayments().Wait();
            EventTestsRepository.StoreLastProcessedEventId("PeriodEnd", "PERIOD2").Wait();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole();
            WorkerRole.OnStart();
        }

        private void ClearSubstituteApis()
        {
            EventsApi.ClearSetup();
        }
    }
}
