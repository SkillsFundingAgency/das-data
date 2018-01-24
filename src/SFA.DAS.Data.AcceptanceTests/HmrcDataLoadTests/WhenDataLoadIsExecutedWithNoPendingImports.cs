using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithNoPendingImports : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenAnErrorShouldBeAddedToProcess_LogTableWithTheCorrectValues()
        {
            HmrcDataTestsRepository.InsertIntoLoadControl(new LoadControlRecord
            {
                SourceFile_Name = "TestImport.psv",
                SourceFile_Status = "Complete",
                InsertDate = DateTime.Now
            }).Wait();
            
            HmrcDataTestsRepository.ExecuteLoadData().Wait();
            
            var processLogContents = HmrcDataTestsRepository.GetProcessLog().Result;

            processLogContents.Count().Should().Be(1);
            processLogContents.First().ProcessEventName.Should().Be("No Source File ID to load");
            processLogContents.First().ProcessEventDescription.Should().Be("No records loaded");
        }

        [Test]
        public void ThenDataLiveShouldRemainEmpty()
        {
            HmrcDataTestsRepository.InsertIntoLoadControl(new LoadControlRecord
            {
                SourceFile_Name = "TestImport.psv",
                SourceFile_Status = "Complete",
                InsertDate = DateTime.Now
            }).Wait();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord() {Record_ID = 1, SchemePAYERef = "999/RD11111"}).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            HmrcDataTestsRepository.GetDataLiveCount().Result.Should().Be(0);
        }
    }
}