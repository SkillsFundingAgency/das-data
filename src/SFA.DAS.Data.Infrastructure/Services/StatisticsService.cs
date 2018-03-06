using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.EasRdsStatistics;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMediator _mediator;
        private readonly ILog _log;
        private readonly IEasStatisticsHandler _easStatisticsHandler;
        private readonly IStatisticsRepository _repository;

        public StatisticsService([Inject] ILog log,
            [Inject] IEasStatisticsHandler easStatisticsHandler, 
            [Inject] IStatisticsRepository repository,
            [Inject]IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _easStatisticsHandler = easStatisticsHandler ?? throw new ArgumentNullException(nameof(easStatisticsHandler));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CollateEasMetrics()
        {
            var statistics = await RetrieveAesStatisticsFromTheApi();

            if (statistics == null) 
            {
                _log.Debug("Quitting operation as it failed to retrieve the Aes Statistics");
                return;
            }

            var rdsStatistics = await RetrieveRelatedAesStatisticsFromRds();

            if (rdsStatistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the related Aes statistics from Rds");
            }

            var savedSuccessfully = await SaveTheStatisticsToRds(statistics, rdsStatistics);

            if (!savedSuccessfully)
            {
                _log.Debug("Quitting operation as it failed to save the statistics");
            }

            // message on queue
            _log.Debug("Placing message on the queue");
        }

        private async Task<bool> SaveTheStatisticsToRds(EasStatisticsModel statistics, RdsStatisticsForEasModel rdsStatistics)
        {
            var response = await _mediator.SendAsync<EasRdsStatisticsCommandResponse>(new EasRdsStatisticsCommand
            {
                EasStatisticsModel = statistics,
                RdsStatisticsForEasModel = rdsStatistics
            });

            return response.OperationSuccessful;
        }

        private async Task<RdsStatisticsForEasModel> RetrieveRelatedAesStatisticsFromRds()
        {
            RdsStatisticsForEasModel rdsStatistics = null;

            _log.Debug("Gathering statistics for the equivalent EAS stats in RDS");
            try
            {
                rdsStatistics = await _repository.RetrieveEquivalentEasStatisticsFromRds();
            }
            catch (SqlException exception)
            {
                _log.Error(exception, "Failed to retrieve the equivalent AES stats from the RDS Database");
            }

            return rdsStatistics;
        }

        private async Task<EasStatisticsModel> RetrieveAesStatisticsFromTheApi()
        {
            _log.Debug("Gathering statistics for the EAS area of the system");
            EasStatisticsModel statistics = null;

            try
            {
                statistics = await _easStatisticsHandler.Handle();
            }
            catch (HttpRequestException httpRequestException)
            {
                _log.Error(httpRequestException, "Failed to retrieve EAS stats from the API");
            }

            return statistics;
        }
    }
}
