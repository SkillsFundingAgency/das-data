using System;
using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions;
using SFA.DAS.Data.Functions.Commands;
using SFA.DAS.Data.Functions.Commands.CommitmentRdsStatistics;
using SFA.DAS.Data.Functions.Commands.EasRdsStatistics;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IEventsApi _eventsApi;
        private readonly IMediator _mediator;
        private readonly ILog _log;
        private readonly IEasStatisticsHandler _easStatisticsHandler;
        private readonly IStatisticsRepository _repository;
        private readonly ICommitmentsStatisticsHandler _commitmentsStatisticsHandler;

        public StatisticsService([Inject] ILog log,
            [Inject] IEasStatisticsHandler easStatisticsHandler, 
            [Inject] IStatisticsRepository repository,
            [Inject] IMediator mediator,
            [Inject] IEventsApi eventsApi,
            [Inject] ICommitmentsStatisticsHandler commitmentsStatisticsHandler)
        {
            _eventsApi = eventsApi ?? throw new ArgumentNullException(nameof(eventsApi));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _easStatisticsHandler = easStatisticsHandler ?? throw new ArgumentNullException(nameof(easStatisticsHandler));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _commitmentsStatisticsHandler = commitmentsStatisticsHandler ?? throw new ArgumentNullException(nameof(commitmentsStatisticsHandler));
        }

        public async Task<IProcessingCompletedMessage> CollateEasMetrics()
        {
            var statistics = await RetrieveAesStatisticsFromTheApi();

            if (statistics == null) 
            {
                _log.Debug("Quitting operation as it failed to retrieve the Aes Statistics");
                return null;
            }

            var rdsStatistics = await RetrieveRelatedAesStatisticsFromRds();

            if (rdsStatistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the related Aes statistics from Rds");
                return null;
            }

            var savedSuccessfully = await SaveTheStatisticsToRds<EasStatisticsModel, RdsStatisticsForEasModel, EasRdsStatisticsCommandResponse, EasRdsStatisticsCommand>(statistics, rdsStatistics);

            if (!savedSuccessfully)
            {
                _log.Debug("Quitting operation as it failed to save the statistics");
                return null;
            }
            //else
            //{
            //    //await AddMessageToQueueToNotifyThatAesDataGatheringIsComplete<EasProcessingCompletedEvent>();

            //}
            return new EasProcessingCompletedMessage
            {
                ProcessingCompletedAt = DateTime.UtcNow
            };
        }

        public async Task<IProcessingCompletedMessage> CollateCommitmentStatisticsMetrics()
        {
            var statistics = await RetrieveCommitmentsStatisticsFromTheApi();

            if (statistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the Commitment Statistics");
                return null;
            }

            var rdsStatistics = await RetrieveRelatedCommitmentsStatisticsFromRds();

            if (rdsStatistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the related Aes statistics from Rds");
                return null;
            }

            var savedSuccessfully = await SaveTheStatisticsToRds<CommitmentsStatisticsModel, RdsStatisticsForCommitmentsModel, CommitmentRdsStatisticsCommandResponse, CommitmentRdsStatisticsCommand>(statistics, rdsStatistics);

            if (!savedSuccessfully)
            {
                _log.Debug("Quitting operation as it failed to save the statistics");
                return null;
            }
            //else
            //{
            //   // await AddMessageToQueueToNotifyThatAesDataGatheringIsComplete<CommitmentProcessingCompletedEvent>();
            //    return null;
            //}

            return null;
        }

        private async Task AddMessageToQueueToNotifyThatAesDataGatheringIsComplete<TEvent>()
            where TEvent : IProcessingCompletedMessage, new()
        {
            _log.Debug("Placing message on the queue");

            var processingCompleted = new TEvent
            {
                ProcessingCompletedAt = DateTime.UtcNow
            };

            var genericEvent = new GenericEvent
            {
                CreatedOn = DateTime.Now,
                Payload = JsonConvert.SerializeObject(processingCompleted),
                Type = processingCompleted.GetType().Name
            };

            await _eventsApi.CreateGenericEvent(genericEvent);
        }

        private async Task<bool> SaveTheStatisticsToRds<TModel, TRdsModel, TCommandResponse, TCommand>(TModel statistics, TRdsModel rdsStatistics)
            where TModel : IExternalSystemModel
            where TRdsModel : IRdsModel
            where TCommandResponse : ICommandResponse
            where TCommand : IStatisticsCommand<TModel, TRdsModel>, new()
        {
            var response = await _mediator.SendAsync((IAsyncRequest<TCommandResponse>) new TCommand
            {
                ExternalStatisticsModel = statistics,
                RdsStatisticsModel = rdsStatistics
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
            catch (DbException exception)
            {
                _log.Error(exception, "Failed to retrieve the equivalent AES stats from the RDS Database");
            }

            return rdsStatistics;
        }

        private async Task<RdsStatisticsForCommitmentsModel> RetrieveRelatedCommitmentsStatisticsFromRds()
        {
            RdsStatisticsForCommitmentsModel rdsStatistics = null;

            _log.Debug("Gathering statistics for the equivalent commitment stats in RDS");

            try
            {
                rdsStatistics = await _repository.RetrieveEquivalentCommitmentsStatisticsFromRds();
            }
            catch (DbException exception)
            {
                _log.Error(exception, "Failed to retrieve the equivalent commitment stats from the RDS Database");
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

        private async Task<CommitmentsStatisticsModel> RetrieveCommitmentsStatisticsFromTheApi()
        {
            _log.Debug("Gathering statistics for the Commitments area of the system");
            CommitmentsStatisticsModel statistics = null;

            try
            {
                statistics = await _commitmentsStatisticsHandler.Handle();
            }
            catch (HttpRequestException httpRequestException)
            {
                _log.Error(httpRequestException, "Failed to retrieve commitment stats from the API");
            }

            return statistics;
        }
    }
}
