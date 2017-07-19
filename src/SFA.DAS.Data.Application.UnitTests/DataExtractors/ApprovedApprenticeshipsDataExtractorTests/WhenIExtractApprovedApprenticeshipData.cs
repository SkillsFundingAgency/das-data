using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.DataExtractors;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.DataExtractors.ApprovedApprenticeshipsDataExtractorTests
{
    [TestFixture]
    public class WhenIExtractApprovedApprenticeshipData
    {
        private const string ExpectedDataType = "apprentices approved";

        private Mock<IPerformancePlatformRepository> _performancePlatformRepository;
        private Mock<IApprenticeshipRepository> _apprenticeshipRepository;
        private ApprovedApprenticeshipsDataExtractor _extractor;

        [SetUp]
        public void Arrange()
        {
            _performancePlatformRepository = new Mock<IPerformancePlatformRepository>();
            _apprenticeshipRepository = new Mock<IApprenticeshipRepository>();

            _extractor = new ApprovedApprenticeshipsDataExtractor(_performancePlatformRepository.Object, _apprenticeshipRepository.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task ThenTheNumberOfNewPayeSchemesSinceTheLastRunIsReturned()
        {
            var numberOfRecordsInLastRun = 45;
            var currentNumberOfRecords = 50;
            _performancePlatformRepository.Setup(x => x.GetNumberOfRecordsFromLastRun(ExpectedDataType)).ReturnsAsync(numberOfRecordsInLastRun);
            _apprenticeshipRepository.Setup(x => x.GetTotalNumberOfAgreedApprenticeships()).ReturnsAsync(currentNumberOfRecords);

            var extractDateTime = DateTime.Now;

            var data = await _extractor.Extract(extractDateTime);

            Assert.AreEqual(ExpectedDataType, data.Type);
            Assert.AreEqual(extractDateTime.AddDays(-1).Date, data.Timestamp);
            Assert.AreEqual(5, data.RecordsSinceLastRun);
            Assert.AreEqual(currentNumberOfRecords, data.TotalNumberOfRecords);
        }
    }
}
