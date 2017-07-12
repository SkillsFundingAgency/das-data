using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.DataExtractors;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.DataExtractors.AccountsDataExtractorTests
{
    [TestFixture]
    public class WhenIExtractAccountData
    {
        private const string ExpectedDataType = "account registered";

        private Mock<IPerformancePlatformRepository> _performancePlatformRepository;
        private Mock<IAccountRepository> _accountRepository;
        private AccountsDataExtractor _extractor;

        [SetUp]
        public void Arrange()
        {
            _performancePlatformRepository = new Mock<IPerformancePlatformRepository>();
            _accountRepository = new Mock<IAccountRepository>();

            _extractor = new AccountsDataExtractor(_performancePlatformRepository.Object, _accountRepository.Object);
        }

        [Test]
        public async Task ThenTheNumberOfNewAccountsSinceTheLastRunIsReturned()
        {
            var numberOfRecordsInLastRun = 45;
            var currentNumberOfRecords = 50;
            _performancePlatformRepository.Setup(x => x.GetNumberOfRecordsFromLastRun(ExpectedDataType)).ReturnsAsync(numberOfRecordsInLastRun);
            _accountRepository.Setup(x => x.GetTotalNumberOfAccounts()).ReturnsAsync(currentNumberOfRecords);

            var extractDateTime = DateTime.Now;

            var data = await _extractor.Extract(extractDateTime);

            Assert.AreEqual(ExpectedDataType, data.Type);
            Assert.AreEqual(extractDateTime.AddDays(-1).Date, data.Timestamp);
            Assert.AreEqual(5, data.RecordsSinceLastRun);
            Assert.AreEqual(currentNumberOfRecords, data.TotalNumberOfRecords);
        }
    }
}
