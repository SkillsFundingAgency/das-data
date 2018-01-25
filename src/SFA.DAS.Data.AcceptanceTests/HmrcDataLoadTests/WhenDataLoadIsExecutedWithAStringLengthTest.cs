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

            HmrcDataTestsRepository.InsertIntoDataQualityTests(new DataQualityTestRecord
            {
                ColumnName = "SchemePAYERef",
                ColumnNullable = true,
                ColumnType = "NVARCHAR",
                ColumnLength = 5,
                RunColumnTests = true
            }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var qualityLogs = HmrcDataTestsRepository.GetQualityLogs().Result.ToList();

            qualityLogs.Count().Should().Be(1);
            qualityLogs.First().ColumnName.Should().Be("SchemePAYERef");
            qualityLogs.First().ErrorMessage.Should().Be("String length exceeds Specification Actual: 9 Against spec size of: 5");
            //qualityLogs.First().ShouldBeEquivalentTo(new QualityLog(){ColumnName = "SchemePAYERef", ErrorMessage = "String length exceeds Specification Actual: 9 Against spec size of: 5", TestName = "String Length"});
        }
    }
}