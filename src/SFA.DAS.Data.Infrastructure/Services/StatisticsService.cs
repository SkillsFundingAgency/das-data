using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ILog _log;
        private readonly IEasStatisticsHandler _easStatisticsHandler;
        private readonly IStatisticsRepository _repository;

        public StatisticsService([Inject] ILog log,
            [Inject] IEasStatisticsHandler easStatisticsHandler, [Inject] IStatisticsRepository repository)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _easStatisticsHandler = easStatisticsHandler ?? throw new ArgumentNullException(nameof(easStatisticsHandler));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CollateEasMetrics()
        {
            _log.Debug("Gathering statistics for the EAS area of the system");
            EasStatisticsModel statistics = null;
            var apiCallSuccessful = true;

            try
            {
                statistics = await _easStatisticsHandler.Handle();
            }
            catch (HttpRequestException httpRequestException)
            {
                apiCallSuccessful = false;
                _log.Error(httpRequestException, "Failed to retrieve EAS stats from the API");
            }

            if (apiCallSuccessful)
            {
                _log.Debug("Gathering statistics for the equivalent EAS stats in RDS");
                var rdsStatistics = await _repository.RetrieveEquivalentEasStatisticsFromRds();

                _log.Debug("Saving the statistics");
                await _repository.SaveEasStatistics(statistics, rdsStatistics);

                // message on queue
                _log.Debug("Placing message on the queue");
            }
        }
    }
}
