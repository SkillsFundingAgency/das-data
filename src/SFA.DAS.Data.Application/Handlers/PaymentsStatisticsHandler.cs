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
    public class PaymentsStatisticsHandler : IPaymentStatisticsHandler
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IDataConfiguration _configuration;
        private readonly ILog _logger;

        public PaymentsStatisticsHandler(IHttpClientWrapper httpClientWrapper, IDataConfiguration configuration, ILog logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));

        }

        public async Task<PaymentStatisticsModel> Handle()
        {
            _logger.Debug("Contacting the payment statistics End point");
            var response = await _httpClientWrapper.GetAsync(_configuration.PaymentsStatisticsEndPoint.ToUri(), Constants.ContentTypeValue);

            _logger.Debug($"The API returned a response with status code {response.StatusCode}");
            var model = await _httpClientWrapper.ReadResponse<PaymentStatisticsModel>(response);

            return model;
        }
    }
}
