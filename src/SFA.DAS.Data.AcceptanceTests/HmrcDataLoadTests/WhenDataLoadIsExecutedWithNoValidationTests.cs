using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithNoValidationTests : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenTheNumberOfRecordsInDataLiveShouldMatchDataStaging()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord {Record_ID = 1, SchemePAYERef = "999/RD11111"}).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            HmrcDataTestsRepository.GetDataLiveCount().Result.Should().Be(2);
        }

        

        [Test]
        public void ThenDataStagingSourceFileIdIsUpdatedFromLoadControl()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 1, SchemePAYERef = "999/RD11111" }).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var stagingRecords = HmrcDataTestsRepository.GetStagingRecords().Result;

            stagingRecords.All(r => r.SourceFile_ID == 1).Should().BeTrue();
        }


        [Test]
        public void ThenLoadControl_Flag_Flag_LoadedSuccessfullyintoLiveTable_IsSetToTrue()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 1, SchemePAYERef = "999/RD11111" }).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;

            loadControl.Flag_LoadedSuccessfullyintoLiveTable.Should().BeTrue();
        }

        [Test]
        public void ThenDataHistoryIsUpdatedWithStagingRecords()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 1, SchemePAYERef = "999/RD11111" }).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112" }).Wait();

            HmrcDataTestsRepository.InsertIntoHistory(new DataHistoryRecord { Record_ID = 100, SourceFile_ID = 99,  SchemePAYERef = "999/RD11112" }).Wait();


            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;

            loadControl.Flag_LoadedSuccessfullyintoHistoryTable.Should().BeTrue();
        }

        [Test]
        public void ThenLoadControl_Flag_Flag_LoadedSuccessfullyintoHistoryTable_IsSetToTrue() 
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 1, SchemePAYERef = "999/RD11111" }).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;

            loadControl.Flag_LoadedSuccessfullyintoHistoryTable.Should().BeTrue();
        }

        [Test]
        public void ThenLoadControl_SourceFile_Status_IsSetToComplete()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 1, SchemePAYERef = "999/RD11111" }).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;

            loadControl.SourceFile_Status.Should().Be("Complete");
        }
    }
}