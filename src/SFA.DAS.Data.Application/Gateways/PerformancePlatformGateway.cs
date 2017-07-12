using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Gateways;

namespace SFA.DAS.Data.Application.Gateways
{
    public class PerformancePlatformGateway : IPerformancePlatformGateway
    {
        private readonly IDataConfiguration _configuration;
        private readonly IHttpClient _httpClient;

        public PerformancePlatformGateway(IDataConfiguration configuration, IHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task SendData(IEnumerable<PerformancePlatformData> data)
        {
            var serializedData = JsonConvert.SerializeObject(data);

            await _httpClient.PostAsync(_configuration.PerformancePlatform.Url, serializedData, _configuration.PerformancePlatform.Token);
        }
    }
}
