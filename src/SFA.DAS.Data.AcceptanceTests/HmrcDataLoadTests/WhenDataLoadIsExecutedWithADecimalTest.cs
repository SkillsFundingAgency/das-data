using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithADecimalTest : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTests()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord{ EnglishFraction = "2123312.121212"}).Wait();

            InsertQualityTestRecord(stopLoadOnError: false);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("EnglishFraction", "Decimal places do not match specification. Actual: 2123312.121212 Expected Decimal Places: 5"); 
            AssertLoadCompleted();
        }

        [Test]
        public void ThenValidationPassesAreNotLoggedIntoConfigurationDataQualityTests_AndLiveTableIsInserted()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { EnglishFraction = "678.34567" }).Wait();

            InsertQualityTestRecord(stopLoadOnError: true);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            HmrcDataTestsRepository.GetQualityLogs().Result.Count().Should().Be(0, because: "there should be no quality test failures");
            
            AssertLoadCompleted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { EnglishFraction = "2123312.121212" }).Wait();

            InsertQualityTestRecord(stopLoadOnError: true);

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("EnglishFraction", "Decimal places do not match specification. Actual: 2123312.121212 Expected Decimal Places: 5");
            AssertLoadHalted();
        }
        private void InsertQualityTestRecord(bool stopLoadOnError)
        {
            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "EnglishFraction",
                ColumnNullable = true,
                ColumnType = "DECIMAL",
                ColumnLength = 18,
                ColumnPrecision = 5,
                RunColumnTests = true,
                StopLoadIfTestDecimalPlaces = stopLoadOnError
            }).Wait();
        }
    }
}