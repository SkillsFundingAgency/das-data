using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Handlers
{
    public class CommitmentsStatisticsHandler : ICommitmentsStatisticsHandler
    {
        private readonly ILog _logger;
        private readonly IDataConfiguration _configuration;
        private readonly IHttpClientWrapper _httpClientWrapper;

        public CommitmentsStatisticsHandler(IHttpClientWrapper httpClientWrapper, IDataConfiguration configuration, ILog logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));
        }

        public async Task<CommitmentsStatisticsModel> Handle()
        {
            _logger.Debug("Contacting the commitments statistics End point");
            var response = await _httpClientWrapper.GetAsync(_configuration.CommitmentsStatisticsEndPoint.ToUri(), Constants.ContentTypeValue);

            _logger.Debug($"The API returned a response with status code {response.StatusCode}");
            var model = await _httpClientWrapper.ReadResponse<CommitmentsStatisticsModel>(response);

            return model;
        }
    }
}
