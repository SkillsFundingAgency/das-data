using System;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Handlers
{
    public class EasStatisticsHandler : IEasStatisticsHandler
    {
        private readonly ILog _logger;
        private readonly IAccountGateway _accountGateway;

        public EasStatisticsHandler(IAccountGateway accountGateway, ILog logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountGateway = accountGateway;
        }

        public async Task<EasExternalModel> Handle()
        {
            _logger.Debug("Contacting the EAS statistics end point");
            var statistics = await _accountGateway.GetStatistics();
            return Map(statistics);
        }

        private EasExternalModel Map(StatisticsViewModel model)
        {
            var externalModel = new EasExternalModel()
            {
                TotalAccounts = model.TotalAccounts,TotalAgreements = model.TotalSignedAgreements, TotalLegalEntities = model.TotalActiveLegalEntities, TotalPAYESchemes = model.TotalPayeSchemes, TotalPayments = model.TotalPaymentsThisYear
            };

            return externalModel;
        }
    }
}
