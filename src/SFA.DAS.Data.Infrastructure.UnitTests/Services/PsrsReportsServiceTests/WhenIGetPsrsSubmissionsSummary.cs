using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models.PSRS;
using SFA.DAS.Data.Infrastructure.Services;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.PsrsReportsServiceTests
{
    [TestFixture]
    public class WhenIGetPsrsSubmissionsSummary
    {
        private Mock<IPsrsRepository> _psrsRepositoryMock;
        private Mock<IPsrsExternalRepository> _psrsExternalRepositoryMock;
        private PsrsReportsService _service;

        [SetUp]
        public void Arrange()
        {
            _psrsRepositoryMock = new Mock<IPsrsRepository>();
            _psrsExternalRepositoryMock = new Mock<IPsrsExternalRepository>();

            _service = new PsrsReportsService(_psrsExternalRepositoryMock.Object, _psrsRepositoryMock.Object);

            var reportSubmissionsSummary = new ReportSubmissionsSummary
            {
                ToDate = DateTime.UtcNow,
                ReportingPeriod = "1718",
                InProcessTotals = 1,
                ViewedTotals = 2,
                SubmittedTotals = 3,
                Total = 6,
            };
            _psrsExternalRepositoryMock.Setup(x => x.GetSubmissionsSummary())
                .Returns(Task.FromResult(reportSubmissionsSummary));
        }

        [Test]
        public void ThenSaveSubmissionsSummarys()
        {
            _service.CreatePsrsReportSubmissionsSummary();
            _psrsRepositoryMock.Verify(v => v.SaveSubmissionsSummary(It.IsAny<ReportSubmissionsSummary>()));
        }
    }
}
