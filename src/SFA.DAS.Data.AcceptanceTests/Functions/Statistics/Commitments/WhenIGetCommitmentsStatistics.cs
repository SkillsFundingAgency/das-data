using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Statistics.Commitments
{
    [TestFixture]
    public class WhenIGetCommitmentsStatistics : CommitmentsTestBase
    {
        [Test]
        public async Task ThenSaveCommitmentsStatistics()
        {
            await SetupDatabase();

            var commitmentsExternalModel = new CommitmentsExternalModel()
            {
                TotalCohorts = 1, TotalApprenticeships = 1, ActiveApprenticeships = 1
            };
            CommitmentsStatisticsHandlerMock.Setup(x => x.Handle()).Returns(Task.FromResult(commitmentsExternalModel));

            await DAS.Data.Functions.Statistics.GetCommitmentStatisticsFunction.Run(null, Log, StatisticsService);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var commitmentsStatisticsCount = await StatisticsTestsRepository.GetNumberOfLatestActiveApprenticeships();
            commitmentsStatisticsCount += await StatisticsTestsRepository.GetNumberOfLatestTotalApprenticeships();
            commitmentsStatisticsCount += await StatisticsTestsRepository.GetNumberOfLatestTotalCohorts();

            return commitmentsStatisticsCount == 3;
        }
    }
}
