using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithAStringLengthTest : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTests()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord{SchemePAYERef = "123456789"}).Wait();

            InsertQualityTestRecord(false);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("SchemePAYERef", "String length exceeds Specification. Actual: 9 Against spec size: 5");
            AssertLoadCompleted();
        }

        [Test]
        public void ThenValidationPassesAreNotLoggedIntoConfigurationDataQualityTests()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { SchemePAYERef = "12345" }).Wait();

            InsertQualityTestRecord(false);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            HmrcDataTestsRepository.GetQualityLogs().Result.Count().Should().Be(0, because: "there should be no quality test failures");

            AssertLoadCompleted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { SchemePAYERef = "123456789" }).Wait();

            InsertQualityTestRecord(true);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("SchemePAYERef", "String length exceeds Specification. Actual: 9 Against spec size: 5");
            AssertLoadHalted();
        }

        private void InsertQualityTestRecord(bool stopLoadOnError)
        {
            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "SchemePAYERef",
                ColumnNullable = true,
                ColumnType = "NVARCHAR",
                ColumnLength = 5,
                RunColumnTests = true,
                StopLoadIfTestTextLength = stopLoadOnError
            }).Wait();
        }
    }
}