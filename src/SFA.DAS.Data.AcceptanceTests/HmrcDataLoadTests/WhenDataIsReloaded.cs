using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;

namespace SFA.DAS.Data.AcceptanceTests.HmrcDataLoadTests
{
    [TestFixture]
    public class WhenDataIsReloaded : HmrcDataLoadTestsBase
    {
        [Test]
        public void ThenExistingSourceFileIdDataInHistoryDeleteLogIsCreated()
        {
            InsertPendingLoadControl();

            HmrcDataTestsRepository.InsertIntoHistory(new DataHistoryRecord { SourceFile_ID = 1, SchemePAYERef = "" }).Wait();
            HmrcDataTestsRepository.InsertIntoHistory(new DataHistoryRecord { SourceFile_ID = 1, SchemePAYERef = "" }).Wait();

            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var logs = HmrcDataTestsRepository.GetProcessLog().Result;

            logs.Any(l => l.ProcessEventName == "History Table Records Deleted for SourceFile_ID - 1")
                .Should().BeTrue();
        }

        [Test]
        public void ThenExistingLogFileDeletingRecordsIsNotCreated()
        {
            InsertPendingLoadControl();
            
            HmrcDataTestsRepository.ExecuteLoadData().Wait();

            var logs = HmrcDataTestsRepository.GetProcessLog().Result;

            logs.Any(l => l.ProcessEventName == "History Table Records Deleted for SourceFile_ID - 1")
                .Should().BeFalse();
        }
    }
}