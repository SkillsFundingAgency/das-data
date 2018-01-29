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
    }
}