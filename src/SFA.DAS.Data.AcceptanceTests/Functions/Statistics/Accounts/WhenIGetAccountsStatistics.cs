using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Statistics.Accounts
{
    [TestFixture]
    public class WhenIGetAccountsStatistics : AccountsTestBase
    {
        [Test]
        public async Task ThenSaveAccountsStatistics()
        {
            await SetupDatabase();

            var accountsExternalModel = new EasExternalModel()
            {
                TotalAccounts = 1, TotalPayments = 1, TotalAgreements = 1, TotalLegalEntities = 1, TotalPAYESchemes = 1
            };
            EasStatisticsHandlerMock.Setup(x => x.Handle()).Returns(Task.FromResult(accountsExternalModel));

            await DAS.Data.Functions.Statistics.GetAccountStatisticsFunction.Run(null, Log, StatisticsService);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var accountsStatisticsCount = await StatisticsTestsRepository.GetNumberOfTotalAccounts();
            accountsStatisticsCount += await StatisticsTestsRepository.GetNumberOfTotalPayments();
            accountsStatisticsCount += await StatisticsTestsRepository.GetNumberOfTotalAgreements();
            accountsStatisticsCount += await StatisticsTestsRepository.GetNumberOfTotalLegalEntities();
            accountsStatisticsCount += await StatisticsTestsRepository.GetNumberOfTotalPayeSchemes();

            return accountsStatisticsCount == 5;
        }
    }
}
