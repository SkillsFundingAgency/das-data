using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Gateways;
using SFA.DAS.Data.Application.Interfaces;

namespace SFA.DAS.Data.Application.UnitTests.Gateways.PerformancePlatformGatewayTests
{
    [TestFixture]
    public class WhenISendPerformanceData
    {
        private DataConfiguration _configuration;
        private Mock<IHttpClient> _httpClient;
        private PerformancePlatformGateway _gateway;

        [SetUp]
        public void Arrange()
        {
            _configuration = new DataConfiguration { PerformancePlatform = new PerformancePlatformConfiguration { Url = "http://localhost/test", Token = "jksnfdg843utnergjg" } };
            _httpClient = new Mock<IHttpClient>();
            _gateway = new PerformancePlatformGateway(_configuration, _httpClient.Object);
        }

        [Test]
        public async Task ThenTheDataIsSentToThePerformancePlatform()
        {
            var data = new List<PerformancePlatformData> { new PerformancePlatformData(DateTime.Now, "Type1", 10, 20), new PerformancePlatformData(DateTime.Now, "Type2", 20, 30) };

            await _gateway.SendData(data);

            _httpClient.Verify(x => x.PostAsync(_configuration.PerformancePlatform.Url, JsonConvert.SerializeObject(data), _configuration.PerformancePlatform.Token));
        }
    }
}
