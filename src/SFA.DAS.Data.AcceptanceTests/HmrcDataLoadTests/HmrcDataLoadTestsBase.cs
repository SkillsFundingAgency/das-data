using System;
using System.Linq;
using FluentAssertions;
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
            HmrcDataTestsRepository.DeleteQualityTests().Wait();
            HmrcDataTestsRepository.DeleteQualityLog().Wait();
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

        protected void AssertTestFailLogged(string expectedColumnName, string expectedErrorMessage)
        {
            var qualityLogs = HmrcDataTestsRepository.GetQualityLogs().Result.ToList();

            qualityLogs.Count().Should().Be(1, because: "a Quality Test Log record should have been created");
            qualityLogs.First().ColumnName.Should().Be(expectedColumnName);
            qualityLogs.First().ErrorMessage.Should().Be(expectedErrorMessage);
        }

        protected void AssertLoadHalted()
        {
            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;
            loadControl.SourceFile_Status.Should().Be("Failed");

            var processLogs = HmrcDataTestsRepository.GetProcessLogs().Result;
            processLogs.Any(l => l.ProcessEventName == "ERROR Data Not loaded Data Quality Issues").Should().BeTrue();

            HmrcDataTestsRepository.GetDataLiveCount().Result.Should().Be(0, because: "no Live records should be inserted");
        }

        protected void AssertLoadCompleted()
        {
            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;
            loadControl.SourceFile_Status.Should().Be("Complete");
        }
    }
}