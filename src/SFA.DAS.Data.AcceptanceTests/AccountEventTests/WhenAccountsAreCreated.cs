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
    public class WhenAccountsAreCreated
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
            var timeout = DateTime.Now.AddSeconds(60);
            while (DateTime.Now < timeout)
            {
                var isDatabaseInExpectedState = IsDatabaseInExpectedState();
                isDatabaseInExpectedState.Wait();
                if (isDatabaseInExpectedState.Result)
                {
                    databaseAsExpected = true;
                    break;
                }
                Thread.Sleep(100);
            }

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await _eventTestsRepository.GetLastProcessedEventId("AccountEvents");
            if (lastProcessedEventId != 4)
            {
                return false;
            }

            var numberOfRegistrations = await _eventTestsRepository.GetNumberOfAccounts();
            if (numberOfRegistrations != 2)
            {
                return false;
            }

            var numberOfLegalEntities = await _eventTestsRepository.GetNumberOfLegalEntities();
            if (numberOfLegalEntities != 3)
            {
                return false;
            }

            var numberOfPayeSchemes = await _eventTestsRepository.GetNumberOfPayeSchemes();
            if (numberOfPayeSchemes != 3)
            {
                return false;
            }

            return true;
        }

        private void ConfigureAccountsApi(List<AccountEventView> events)
        {
            _accountsApi.SetupGet("api/accounts/ABC123",
                new AccountDetailViewModelBuilder().WithDasAccountId("ABC123")
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/legalentities/123"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/payeschemes/1234"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ABC123/payeschemes/5678"))
                    .Build());
            _accountsApi.SetupGet("api/accounts/ZZZ999",
                new AccountDetailViewModelBuilder().WithDasAccountId("ZZZ999")
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ZZZ999/legalentities/9876"))
                    .WithLegalEntity(new ResourceViewModelBuilder().WithHref("api/accounts/ZZZ999/legalentities/5432"))
                    .WithPayeScheme(new ResourceViewModelBuilder().WithHref("api/accounts/ZZZ999/payeschemes/9876"))
                    .Build());

            _accountsApi.SetupGet("api/accounts/ABC123/legalentities/123", new LegalEntityViewModelBuilder().WithDasAccountId("ABC123").WithLegalEntityId(123).Build());
            _accountsApi.SetupGet("api/accounts/ABC123/payeschemes/1234", new PayeSchemeViewModelBuilder().WithDasAccountId("ABC123").WithRef("1234").Build());
            _accountsApi.SetupGet("api/accounts/ABC123/payeschemes/5678", new PayeSchemeViewModelBuilder().WithDasAccountId("ABC123").WithRef("5678").Build());
            _accountsApi.SetupGet("api/accounts/ZZZ999/legalentities/9876", new LegalEntityViewModelBuilder().WithDasAccountId("ZZZ999").WithLegalEntityId(9876).Build());
            _accountsApi.SetupGet("api/accounts/ZZZ999/legalentities/5432", new LegalEntityViewModelBuilder().WithDasAccountId("ZZZ999").WithLegalEntityId(5432).Build());
            _accountsApi.SetupGet("api/accounts/ZZZ999/payeschemes/9876", new PayeSchemeViewModelBuilder().WithDasAccountId("ZZZ999").WithRef("9876").Build());
        }

        private List<AccountEventView> ConfigureEventsApi()
        {
            var events = new List<AccountEventView>
            {
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    ResourceUri = "api/accounts/ABC123",
                    Event = "Account Created"
                },
                new AccountEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    ResourceUri = "api/accounts/ZZZ999",
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
