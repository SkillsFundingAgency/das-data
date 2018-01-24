using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithNoPendingImports
    {
        private HmrcDataTestsRepository _hmrcDataTestsRepository;

        [Test]
        public void ThenAnErrorShouldBeAddedToProcess_LogTableWithTheCorrectValues()
        {
            // Clear tables
            SetupDatabase();
            
            // Create record in Load_Control with SourceFile_Status as Complete
            _hmrcDataTestsRepository.InsertIntoLoadControl(new LoadControlRecord
            {
                SourceFile_Name = "TestImport.psv",
                SourceFile_Status = "Complete",
                InsertDate = DateTime.Now
            }).Wait();
            
            // Execute Load_Data

            _hmrcDataTestsRepository.ExecuteLoadData().Wait();
            
            // Assert that Process_Log table contains a record that states nothing to do
            var processLogContents = _hmrcDataTestsRepository.GetProcessLog().Result;

            processLogContents.Count().Should().Be(1);
            processLogContents.First().ProcessEventName.Should().Be("No Source File ID to load");
            processLogContents.First().ProcessEventDescription.Should().Be("No records loaded");
        }

        [Test]
        public void ThenDataLiveShouldRemainEmpty()
        {
            // Clear tables
            SetupDatabase();

            // Create record in Load_Control with SourceFile_Status as Complete
            _hmrcDataTestsRepository.InsertIntoLoadControl(new LoadControlRecord
            {
                SourceFile_Name = "TestImport.psv",
                SourceFile_Status = "Complete",
                InsertDate = DateTime.Now
            }).Wait();

            _hmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord() {Record_ID = 1, SchemePAYERef = "999/RD11111"}).Wait();

            _hmrcDataTestsRepository.ExecuteLoadData().Wait();

            _hmrcDataTestsRepository.GetDataLiveCount().Result.Should().Be(0);
        }

        private void SetupDatabase()
        {
            _hmrcDataTestsRepository = new HmrcDataTestsRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            _hmrcDataTestsRepository.DeleteLive().Wait();
            _hmrcDataTestsRepository.DeleteStaging().Wait();
            _hmrcDataTestsRepository.DeleteLoadControl().Wait();
            _hmrcDataTestsRepository.DeleteProcessLog().Wait();
        }
    }
}