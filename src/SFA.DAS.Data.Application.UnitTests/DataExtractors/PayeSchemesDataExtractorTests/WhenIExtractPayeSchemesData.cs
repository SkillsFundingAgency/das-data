using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.DataExtractors;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.DataExtractors.PayeSchemesDataExtractorTests
{
    [TestFixture]
    public class WhenIExtractPayeSchemesData
    {
        private const string ExpectedDataType = "paye schemes added";

        private Mock<IPerformancePlatformRepository> _performancePlatformRepository;
        private Mock<IPayeSchemeRepository> _payeSchemeRepository;
        private PayeSchemesDataExtractor _extractor;

        [SetUp]
        public void Arrange()
        {
            _performancePlatformRepository = new Mock<IPerformancePlatformRepository>();
            _payeSchemeRepository = new Mock<IPayeSchemeRepository>();

            _extractor = new PayeSchemesDataExtractor(_performancePlatformRepository.Object, _payeSchemeRepository.Object);
        }

        [Test]
        public async Task ThenTheNumberOfNewPayeSchemesSinceTheLastRunIsReturned()
        {
            var numberOfRecordsInLastRun = 45;
            var currentNumberOfRecords = 50;
            _performancePlatformRepository.Setup(x => x.GetNumberOfRecordsFromLastRun(ExpectedDataType)).ReturnsAsync(numberOfRecordsInLastRun);
            _payeSchemeRepository.Setup(x => x.GetTotalNumberOfPayeSchemes()).ReturnsAsync(currentNumberOfRecords);

            var extractDateTime = DateTime.Now;

            var data = await _extractor.Extract(extractDateTime);

            Assert.AreEqual(ExpectedDataType, data.Type);
            Assert.AreEqual(extractDateTime.AddDays(-1).Date, data.Timestamp);
            Assert.AreEqual(5, data.RecordsSinceLastRun);
            Assert.AreEqual(currentNumberOfRecords, data.TotalNumberOfRecords);
        }
    }
}
