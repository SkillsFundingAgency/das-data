﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Functions.Psrs;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.UnitTests.Psrs
{
    [TestFixture]

    public class WhenTheCreatePreviousSubmittedReportsFunctionIsRun
    {
        private Mock<IPsrsReportsService> _reportsService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILog>();
            _reportsService = new Mock<IPsrsReportsService>();
        }

        [Test]
        public async Task ThenTheCreatePsrsSubmittedReportsMethodIsInvoked()
        {
            await InvokeRunMethodOnFunction();

            _reportsService.Verify(o => o.CreatePsrsSubmittedReports(It.IsAny<TimeSpan>()), Times.Once);
        }

        private async Task InvokeRunMethodOnFunction()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, (Uri)null);

            await CreatePreviousSubmittedReportsFunction.RunHttp(request, _logger.Object, _reportsService.Object);
        }
    }
}
