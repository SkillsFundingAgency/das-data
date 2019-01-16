using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models.PSRS;
using SFA.DAS.Data.Infrastructure.Services;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.PsrsReportsServiceTests
{
    [TestFixture]
    public class WhenIGetPsrsSubmittedReports
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

            var submittedReports = new List<ReportSubmitted> { new ReportSubmitted
                {
                    ReportingPeriod = "1718",
                }
            }.AsEnumerable();

            _psrsExternalRepositoryMock.Setup(x => x.GetSubmittedReports(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(submittedReports));
        }

        [Test]
        public void ThenSaveSubmittedReports()
        {
            _service.CreatePsrsSubmittedReports(DateTime.UtcNow.AddMinutes(15));
            _psrsRepositoryMock.Verify(v => v.SaveSubmittedReport(It.IsAny<IEnumerable<ReportSubmitted>>()));
        }
    }
}
