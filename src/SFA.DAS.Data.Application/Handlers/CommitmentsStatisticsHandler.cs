using System;
using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Handlers
{
    public class CommitmentsStatisticsHandler : ICommitmentsStatisticsHandler
    {
        private readonly ILog _logger;
        private readonly ICommitmentsGateway _commitmentsGateway;

        public CommitmentsStatisticsHandler(ICommitmentsGateway commitmentsGateway, ILog logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commitmentsGateway = commitmentsGateway;
        }

        public async Task<CommitmentsExternalModel> Handle()
        {
            _logger.Debug("Contacting the commitments statistics end point");

            var statistics = await _commitmentsGateway.GetStatistics();

            return Map(statistics);
        }

        private CommitmentsExternalModel Map(ConsistencyStatistics model)
        {
            var externalModel = new CommitmentsExternalModel()
            {
                TotalApprenticeships = model.TotalApprenticeships,
                TotalCohorts = model.TotalCohorts,
                ActiveApprenticeships = model.ActiveApprenticeships
            };

            return externalModel;
        }
    }
}
