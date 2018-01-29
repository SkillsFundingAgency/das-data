using System.Linq;
using System.Net.Http;
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

            var qualityLogs = HmrcDataTestsRepository.GetQualityLogs().Result.ToList();

            qualityLogs.Count().Should().Be(1);
            qualityLogs.First().ColumnName.Should().Be("EnglishFraction");
            qualityLogs.First().ErrorMessage.Should().Be("Decimal places do not match specification. Actual: 2123312.121212 Expected Decimal Places: 5");

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;
            loadControl.SourceFile_Status.Should().Be("Complete");
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

            var qualityLogs = HmrcDataTestsRepository.GetQualityLogs().Result.ToList();

            qualityLogs.Count().Should().Be(1);
            qualityLogs.First().ColumnName.Should().Be("EnglishFraction");
            qualityLogs.First().ErrorMessage.Should().Be("Decimal places do not match specification. Actual: 2123312.121212 Expected Decimal Places: 5");

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;
            loadControl.SourceFile_Status.Should().Be("Failed");

            var processLogs = HmrcDataTestsRepository.GetProcessLogs().Result;
            processLogs.Any(l => l.ProcessEventName == "ERROR Data Not loaded Data Quality Issues").Should().BeTrue();

            HmrcDataTestsRepository.GetDataLiveCount().Result.Should().Be(0);
        }
    }
}