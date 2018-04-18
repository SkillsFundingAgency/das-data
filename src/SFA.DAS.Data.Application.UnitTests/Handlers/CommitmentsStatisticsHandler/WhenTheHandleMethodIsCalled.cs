using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Handlers.CommitmentsStatisticsHandler
{
    [TestFixture]
    public class WhenHandleMethodIsCalled
    {
        private Application.Handlers.CommitmentsStatisticsHandler _handler;
        private Mock<IHttpClientWrapper> _httpClientWrapper;
        private Mock<IDataConfiguration> _configuration;
        private Mock<ILog> _logger;
        private const string StatisticsEndPointUri = "https://stats.end/point";

        [SetUp]
        public void Setup()
        {

            _httpClientWrapper = new Mock<IHttpClientWrapper>();
            _configuration = new Mock<IDataConfiguration>();
            _logger = new Mock<ILog>();
            _configuration.SetupGet(o => o.CommitmentsStatisticsEndPoint).Returns(StatisticsEndPointUri);

            _handler = new Application.Handlers.CommitmentsStatisticsHandler(_httpClientWrapper.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        public async Task TheStatisticsApiEndpointIsCalled()
        {
            SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode.Accepted);

            await _handler.Handle();

            _httpClientWrapper.Verify(o => o.GetAsync(StatisticsEndPointUri.ToUri(), It.IsAny<string>()), Times.Once);
        }

        private void SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode statusCode)
        {
            _httpClientWrapper.Setup(o => o.GetAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(statusCode));

            if (statusCode == HttpStatusCode.Accepted)
            {
                _httpClientWrapper.Setup(o => o.ReadResponse<CommitmentsStatisticsModel>(It.IsAny<HttpResponseMessage>()))
                    .ReturnsAsync(
                        new CommitmentsStatisticsModel());
            }
        }

        [Test]
        public async Task ReadsTheResponseFromTheApiCall()
        {
            SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode.Accepted);

            await _handler.Handle();

            _httpClientWrapper.Verify(o => o.ReadResponse<CommitmentsStatisticsModel>(It.IsAny<HttpResponseMessage>()), Times.Once());
        }

        [Test]
        public async Task ThenReturnsTheModel()
        {
            SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode.Accepted);

            var actual = await _handler.Handle();

            Assert.IsNotNull(actual);
        }
    }
}
