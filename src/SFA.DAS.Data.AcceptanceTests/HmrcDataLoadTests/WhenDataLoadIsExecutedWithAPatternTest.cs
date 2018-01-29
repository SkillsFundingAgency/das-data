using System.Linq;
using System.Net.Http;
using FluentAssertions;
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

            var qualityLogs = HmrcDataTestsRepository.GetQualityLogs().Result.ToList();

            qualityLogs.Count().Should().Be(1);
            qualityLogs.First().ColumnName.Should().Be("SchemePAYERef");
            qualityLogs.First().ErrorMessage.Should().Be("Column pattern does not match specification. Actual: DG123 Expected Pattern: [0-9][0-9][0-9]/[A-Z]%");

            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;
            loadControl.SourceFile_Status.Should().Be("Complete");
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

            var qualityLogs = HmrcDataTestsRepository.GetQualityLogs().Result.ToList();

            qualityLogs.Count().Should().Be(1);
            qualityLogs.First().ColumnName.Should().Be("SchemePAYERef");
            qualityLogs.First().ErrorMessage.Should().Be("Column pattern does not match specification. Actual: DG123 Expected Pattern: [0-9][0-9][0-9]/[A-Z]%");


            var loadControl = HmrcDataTestsRepository.GetLoadControl().Result;
            loadControl.SourceFile_Status.Should().Be("Failed");

            var processLogs = HmrcDataTestsRepository.GetProcessLogs().Result;
            processLogs.Any(l => l.ProcessEventName == "ERROR Data Not loaded Data Quality Issues").Should().BeTrue();

            HmrcDataTestsRepository.GetDataLiveCount().Result.Should().Be(0);
        }
    }
}