using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Handlers
{
    public abstract class StatisticsHandlerBase<T> where T : IExternalSystemModel, new()
    {
        protected Mock<IHttpClientWrapper> HttpClientWrapper;
        protected Mock<IDataConfiguration> Configuration;
        protected Mock<ILog> Logger;
        protected const string StatisticsEndPointUri = "https://stats.end/point";

        protected void SetUp(Expression<Func<IDataConfiguration, string>> expression)
        {
            HttpClientWrapper = new Mock<IHttpClientWrapper>();
            Configuration = new Mock<IDataConfiguration>();
            Logger = new Mock<ILog>();

            Configuration.Setup(expression).Returns(StatisticsEndPointUri);
        }

        protected void SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode statusCode)
        {
            HttpClientWrapper.Setup(o => o.GetAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(statusCode));

            if (statusCode == HttpStatusCode.Accepted)
            {
                HttpClientWrapper.Setup(o => o.ReadResponse<T>(It.IsAny<HttpResponseMessage>()))
                    .ReturnsAsync(
                        new T());
            }
        }

        protected abstract Task<dynamic> CallHandleMethodOnHandler();

        [Test]
        public async Task TheStatisticsApiEndpointIsCalled()
        {
            SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode.Accepted);

            await CallHandleMethodOnHandler();

            HttpClientWrapper.Verify(o => o.GetAsync(StatisticsEndPointUri.ToUri(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ReadsTheResponseFromTheApiCall()
        {
            SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode.Accepted);

            await CallHandleMethodOnHandler();

            HttpClientWrapper.Verify(o => o.ReadResponse<T>(It.IsAny<HttpResponseMessage>()), Times.Once());
        }

        [Test]
        public async Task ThenReturnsTheModel()
        {
            SetupWrapperToReturnHttpResponseMessageWithStatusCodeSetTo(HttpStatusCode.Accepted);

            var actual = await CallHandleMethodOnHandler();

            Assert.IsNotNull(actual);
        }
    }
}
