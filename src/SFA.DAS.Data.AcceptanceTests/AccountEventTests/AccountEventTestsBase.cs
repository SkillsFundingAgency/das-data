﻿using System.Threading.Tasks;
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

        protected abstract string EventName { get; }

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
            EventTestsRepository = new EventTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            EventTestsRepository.DeleteAccounts().Wait();
            EventTestsRepository.DeleteLevyDeclarations().Wait();
            EventTestsRepository.DeleteFailedEvents().Wait();
            EventTestsRepository.StoreLastProcessedEventId(EventName, 2).Wait();
        }

        private void StartWorkerRole()
        {
            WorkerRole = new WorkerRole();
            WorkerRole.OnStart();
        }

        private void ClearSubstituteApis()
        {
            DataAcceptanceTests.ClearApiSetup();
        }
    }
}
