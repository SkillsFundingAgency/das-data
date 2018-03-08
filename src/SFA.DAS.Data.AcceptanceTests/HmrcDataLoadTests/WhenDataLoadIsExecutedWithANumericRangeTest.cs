using System.Linq;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithANumericRangeTest : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTests()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { TaxPeriodMonth = "14" }).Wait();

            InsertQualityTestRecord(false);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("TaxPeriodMonth", "Numeric column value outside acceptable values. Actual: 14. Range: 1 - 12");
            AssertLoadCompleted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { TaxPeriodMonth = "14" }).Wait();

            InsertQualityTestRecord(true);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("TaxPeriodMonth", "Numeric column value outside acceptable values. Actual: 14. Range: 1 - 12");
            AssertLoadHalted();
        }

        [Test]
        public void ThenValidationPassesAreNotLoggedIntoConfigurationDataQualityTests_AndLoadTakesPlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { TaxPeriodMonth = "12" }).Wait();

            InsertQualityTestRecord(true);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            HmrcDataTestsRepository.GetQualityLogs().Result.Count().Should().Be(0, because: "there should be no quality test failures");

            AssertLoadCompleted();
        }

        private void InsertQualityTestRecord(bool stopLoadOnError)
        {
            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "TaxPeriodMonth",
                ColumnNullable = true,
                ColumnType = "INT",
                ColumnMinValue = "1",
                ColumnMaxValue = "12",
                RunColumnTests = true,
                StopLoadIfTestValueRange = stopLoadOnError
            }).Wait();
        }
    }
}