﻿using System.Configuration;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.ApiSubstitute;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.Worker;

namespace SFA.DAS.Data.AcceptanceTests.AccountEventTests
{
    public abstract class AccountEventTestsBase
    {
        protected WorkerRole WorkerRole;
        protected EventTestsRepository EventTestsRepository;

        protected WebApiSubstitute AccountsApi => DataAcceptanceTests.AccountsApi;
        protected WebApiSubstitute EventsApi => DataAcceptanceTests.EventsApi;

        [SetUp]
        public void Arrange()
        {
            ClearSubstituteApis();
            StartWorkerRole();
            SetupDatabase();
        }

        private void SetupDatabase()
        {
            EventTestsRepository = new EventTestsRepository(ConfigurationManager.AppSettings["DataConnectionString"]);
            EventTestsRepository.DeleteAccounts().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId("AccountEvents", 2).Wait();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole();
            WorkerRole.OnStart();
        }

        private void ClearSubstituteApis()
        {
            EventsApi.ClearSetup();
            AccountsApi.ClearSetup();
        }
    }
}
