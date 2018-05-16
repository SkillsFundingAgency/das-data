using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Configuration;
using SFA.DAS.Data.Domain;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Client;

namespace SFA.DAS.Data.Application.Handlers
{
    public class PaymentsStatisticsHandler : IPaymentStatisticsHandler
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IDataConfiguration _configuration;
        private readonly IPaymentsEventsApiClient _paymentsEventsApi;
        private readonly ILog _logger;

        public PaymentsStatisticsHandler(IHttpClientWrapper httpClientWrapper, IDataConfiguration configuration, ILog logger, IPaymentsEventsApiClient paymentsEventsApi)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentsEventsApi = paymentsEventsApi;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));

        }

        public async Task<PaymentExternalModel> Handle()

        {
            _logger.Debug("Contacting the payment statistics End point");
            var response = await _paymentsEventsApi.GetPaymentStatistics();

            var model = new PaymentExternalModel()
            {
                ProviderTotalPayments = response.TotalNumberOfPayments,
                ProviderTotalPaymentsWithRequestedPayment = response.TotalNumberOfPaymentsWithRequiredPayment
            };

            return model;
        }
    }
}
