using System;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    public abstract class HmrcDataLoadTestsBase
    {
        protected HmrcDataTestsRepository HmrcDataTestsRepository;

        [SetUp]
        public void Arrange()
        {
            SetupDatabase();
        }

        private void SetupDatabase()
        {
            HmrcDataTestsRepository = new HmrcDataTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            HmrcDataTestsRepository.DeleteLive().Wait();
            HmrcDataTestsRepository.DeleteStaging().Wait();
            HmrcDataTestsRepository.DeleteLoadControl().Wait();
            HmrcDataTestsRepository.DeleteProcessLog().Wait();
            HmrcDataTestsRepository.DeleteHistory().Wait();
        }

        protected void InsertPendingLoadControl()
        {
            HmrcDataTestsRepository.InsertIntoLoadControl(new LoadControlRecord
            {
                SourceFile_Name = "TestImport.psv",
                SourceFile_Status = "Pending",
                InsertDate = DateTime.Now
            }).Wait();
        }
    }
}