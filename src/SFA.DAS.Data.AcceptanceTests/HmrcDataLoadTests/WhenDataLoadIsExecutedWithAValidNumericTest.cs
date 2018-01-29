using System.Linq;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithAValidNumericTest : HmrcDataLoadTestsBase
    {
        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { TaxPeriodMonth = "n" }).Wait();

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "TaxPeriodMonth",
                ColumnNullable = true,
                ColumnType = "INT",
                RunColumnTests = true,
                StopLoadIfTestIsNumeric = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("TaxPeriodMonth", "Numeric type field not Numeric. Actual: n");
            AssertLoadHalted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationPassesAreNotLoggedIntoConfigurationDataQualityTestsAndLoadTakesPlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { TaxPeriodMonth = "2" }).Wait();

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "TaxPeriodMonth",
                ColumnNullable = true,
                ColumnType = "INT",
                RunColumnTests = true,
                StopLoadIfTestIsNumeric = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            HmrcDataTestsRepository.GetQualityLogs().Result.Count().Should().Be(0, because: "there should be no quality test failures");

            AssertLoadCompleted();
        }
    }
}