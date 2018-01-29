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

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "SchemePAYERef",
                ColumnNullable = true,
                ColumnType = "NVARCHAR",
                ColumnLength = 5,
                RunColumnTests = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("SchemePAYERef", "String length exceeds Specification. Actual: 9 Against spec size: 5");
            AssertLoadCompleted();
        }

        [Test]
        public void AndValidationHasFlagToStopLoad_ThenValidationFailuresAreLoggedIntoConfigurationDataQualityTestsAndLoadDoesntTakePlace()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoStaging(new DataStagingRecord { SchemePAYERef = "123456789" }).Wait();

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "SchemePAYERef",
                ColumnNullable = true,
                ColumnType = "NVARCHAR",
                ColumnLength = 5,
                RunColumnTests = true,
                StopLoadIfTestTextLength = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            AssertTestFailLogged("SchemePAYERef", "String length exceeds Specification. Actual: 9 Against spec size: 5");
            AssertLoadHalted();
        }
    }
}