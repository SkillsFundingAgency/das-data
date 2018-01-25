using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithNullCessationDates : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenOnlyNullCessationDatesAreReplacedWith31122999()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 1, SchemePAYERef = "999/RD11111", CessationDate = null}).Wait();

            HmrcDataTestsRepository
                .InsertIntoStaging(new DataStagingRecord { Record_ID = 2, SchemePAYERef = "999/RD11112", CessationDate = "20180125" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var stagingRecords = HmrcDataTestsRepository.GetStagingRecords().Result;

            stagingRecords.Single(r => r.Record_ID == 1).CessationDate.Should().Be("29991231");
            stagingRecords.Single(r => r.Record_ID == 2).CessationDate.Should().NotBe("29991231");
        }
    }
}