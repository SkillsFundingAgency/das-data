using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Http.HttpClientWrapper
{
    [TestFixture]
    public class WhenAttemptingAGetRequest : HttpWrapperTestBase
    {
        [Test]
        public async Task ThenTheGivenUrlIsCalled()
        {
            SetupMessageHandler(HttpStatusCode.OK, (r, c) =>
            {
                Assert.AreEqual(HttpMethod.Get, r.Method);
                Assert.AreEqual(_requestUri, r.RequestUri.ToString());
            });
           
            await _httpClientWrapper.GetAsync(_requestUri.ToUri(), Constants.ContentTypeValue);
        }
    }
}
