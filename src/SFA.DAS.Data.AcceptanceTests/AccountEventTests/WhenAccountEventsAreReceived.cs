using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Data.Worker;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    [TestFixture]
    public class WhenAccountEventsAreReceived
    {
        private WorkerRole _workerRole;
        private WebApiSubstitute _eventsApi;
        private WebApiSubstitute _accountsApi;
        private static EventTestsRepository _eventTestsRepository;

        [SetUp]
        public void Arrange()
        {
            StartSubstituteApis();
            StartWorkerRole();
            SetupDatabase();
        }

        [Test]
        public void ThenTheAccountDetailsAreStored()
        {
            var events = ConfigureEventsApi();
            ConfigureAccountsApi(events);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => _workerRole.Run(), cancellationToken);

            var databaseAsExpected = false;
            var timeout = DateTime.Now.AddSeconds(30);
            while (DateTime.Now < timeout)
            {
                var lastProcessedEventId = _eventTestsRepository.GetLastProcessedEventId("AccountEvents");
                lastProcessedEventId.Wait();
                if (lastProcessedEventId.Result == 4)
                {
                    var numberOfRegistrations = _eventTestsRepository.GetNumberOfAccounts();
                    numberOfRegistrations.Wait();
                    if (numberOfRegistrations.Result == 2)
                    {
                        databaseAsExpected = true;
                    }
                    break;
                }
                Thread.Sleep(100);
            }

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private void ConfigureAccountsApi(List<AccountEventView> events)
        {
            _accountsApi.SetupGet("api/accounts/ABC123", new AccountDetailViewModelBuilder().WithDasAccountId("ABC123").Build());
            _accountsApi.SetupGet("api/accounts/ZZZ999", new AccountDetailViewModelBuilder().WithDasAccountId("ZZZ999").Build());
        }

        private List<AccountEventView> ConfigureEventsApi()
        {
            var events = new List<AccountEventView>
            {
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    EmployerAccountId = "api/accounts/ABC123",
                    Event = "Account Created"
                },
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    EmployerAccountId = "api/accounts/ZZZ999",
                    Event = "Account Created"
                }
            };

            _eventsApi.SetupGet("api/events/accounts?fromEventId=3&pageSize=1000&pageNumber=1", events);
            return events;
        }

        [TearDown]
        public void TearDown()
        {
            _eventsApi.Dispose();
            _accountsApi.Dispose();
        }

        private static void SetupDatabase()
        {
            _eventTestsRepository = new EventTestsRepository(ConfigurationManager.AppSettings["DataConnectionString"]);
            _eventTestsRepository.DeleteAccounts().Wait();
            _eventTestsRepository.DeleteFailedEvents().Wait();
            _eventTestsRepository.StoreLastProcessedEventId("AccountEvents", 2).Wait();
        }

        private void StartWorkerRole()
        {
            _workerRole = new WorkerRole();
            _workerRole.OnStart();
        }

        private void StartSubstituteApis()
        {
            _eventsApi = new WebApiSubstitute(ConfigurationManager.AppSettings["EventsApiBaseUrl"]);
            _accountsApi = new WebApiSubstitute(ConfigurationManager.AppSettings["AccountsApiBaseUrl"]);

            _eventsApi.Start();
            _accountsApi.Start();
        }
    }
}
