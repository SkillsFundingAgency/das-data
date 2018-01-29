using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataLoadIsExecutedWithAPatternTest : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTests()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord{ SchemePAYERef = "DG123"}).Wait();

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "SchemePAYERef",
                ColumnNullable = true,
                ColumnPatternMatching = "[0-9][0-9][0-9]/[A-Z]%",
                RunColumnTests = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("SchemePAYERef", "Column pattern does not match specification. Actual: DG123 Expected Pattern: [0-9][0-9][0-9]/[A-Z]%");
            AssertLoadCompleted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { SchemePAYERef = "DG123" }).Wait();

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "SchemePAYERef",
                ColumnNullable = true,
                ColumnPatternMatching = "[0-9][0-9][0-9]/[A-Z]%",
                RunColumnTests = true,
                StopLoadIfTestPatternMatch = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("SchemePAYERef", "Column pattern does not match specification. Actual: DG123 Expected Pattern: [0-9][0-9][0-9]/[A-Z]%");
            AssertLoadHalted();
        }
    }
}