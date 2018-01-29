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

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "EnglishFraction",
                ColumnNullable = true,
                ColumnType = "DECIMAL",
                ColumnLength = 18,
                ColumnPrecision = 5,
                RunColumnTests = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("EnglishFraction", "Decimal places do not match specification. Actual: 2123312.121212 Expected Decimal Places: 5"); 
            AssertLoadCompleted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { EnglishFraction = "2123312.121212" }).Wait();

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "EnglishFraction",
                ColumnNullable = true,
                ColumnType = "DECIMAL",
                ColumnLength = 18,
                ColumnPrecision = 5,
                RunColumnTests = true,
                StopLoadIfTestDecimalPlaces = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("EnglishFraction", "Decimal places do not match specification. Actual: 2123312.121212 Expected Decimal Places: 5");
            AssertLoadHalted();
        }
    }
}