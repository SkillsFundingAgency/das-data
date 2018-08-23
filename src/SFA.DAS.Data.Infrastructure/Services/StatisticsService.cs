﻿using System;
using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.Data.Application.Commands.CreateCommitmentStatistics;
using SFA.DAS.Data.Application.Commands.CreateEasStatistics;
using SFA.DAS.Data.Application.Commands.CreatePaymentsStatistics;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Application.Messages;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IPaymentStatisticsHandler _paymentStatisticsHandler;
        private readonly IMediator _mediator;
        private readonly ILog _log;
        private readonly IEasStatisticsHandler _easStatisticsHandler;
        private readonly IStatisticsRepository _repository;
        private readonly ICommitmentsStatisticsHandler _commitmentsStatisticsHandler;

        public StatisticsService( ILog log,
            IEasStatisticsHandler easStatisticsHandler,
            IStatisticsRepository repository,
            IMediator mediator,
            ICommitmentsStatisticsHandler commitmentsStatisticsHandler,
            IPaymentStatisticsHandler paymentStatisticsHandler)
        {
            _paymentStatisticsHandler = paymentStatisticsHandler ?? throw new ArgumentNullException(nameof(paymentStatisticsHandler));
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

            var savedSuccessfully = await SaveTheStatisticsToRds<EasExternalModel, EasRdsModel, CreateEasStatisticsCommandResponse, CreateEasStatisticsCommand>(statistics, rdsStatistics);

            if (!savedSuccessfully)
            {
                _log.Debug("Quitting operation as it failed to save the statistics");
                return null;
            }

            return new EasProcessingCompletedMessage
            {
                ProcessingCompletedAt = DateTime.UtcNow
            };
        }

        public async Task<IProcessingCompletedMessage> CollateCommitmentStatisticsMetrics(TraceWriter traceLog)
        {
            traceLog.Info("Calling RetrieveCommitmentsStatisticsFromTheApi", "CollateCommitmentStatisticsMetrics");
            var statistics = await RetrieveCommitmentsStatisticsFromTheApi(traceLog);

            if (statistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the Commitment Statistics");
                traceLog.Info("Quitting operation as it failed to retrieve the Commitment Statistics");
                return null;
            }

            traceLog.Info("Calling RetrieveRelatedCommitmentsStatisticsFromRds");
            var rdsStatistics = await RetrieveRelatedCommitmentsStatisticsFromRds(traceLog);

            if (rdsStatistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the related Commitemtns statistics from Rds");
                traceLog.Info("Quitting operation as it failed to retrieve the related Commitments statistics from Rds, returning null");
                return null;
            }

            traceLog.Info($"RDS statistics received from RDS db, TotalCohorts: {rdsStatistics.TotalCohorts}, TotalApprenticeships: {rdsStatistics.TotalApprenticeships}, ActiveApprenticeships: {rdsStatistics.ActiveApprenticeships}");

            traceLog.Info("Calling SaveTheStatisticsToRds via mediator");
            try
            {
                var savedSuccessfully =
                    await SaveTheStatisticsToRds<CommitmentsExternalModel, CommitmentsRdsModel,
                        CreateCommitmentStatisticsCommandResponse, CreateCommitmentStatisticsCommand>(statistics,
                        rdsStatistics);

                if (!savedSuccessfully)
                {
                    _log.Debug("Quitting operation as it failed to save the statistics");
                    traceLog.Info("Quitting operation as it failed to save the statistics - SaveTheStatisticsToRds returned false");
                    return null;
                }
            }
            catch (Exception ex)
            {
                traceLog.Error("SaveTheStatisticsToRds exception", ex);
                throw;
            }

            traceLog.Info("Stats saved to RDS successfully");

            return new CommitmentProcessingCompletedMessage();
        }

        public async Task<IProcessingCompletedMessage> CollatePaymentStatisticsMetrics()
        {
            var statistics = await RetrievePaymentsStatisticsFromTheApi();

            if (statistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the payment Statistics");
                return null;
            }

            var rdsStatistics = await RetrieveRelatedPaymentsStatisticsFromRds();

            if (rdsStatistics == null)
            {
                _log.Debug("Quitting operation as it failed to retrieve the related Aes statistics from Rds");
                return null;
            }

            var savedSuccessfully = await SaveTheStatisticsToRds<PaymentExternalModel, PaymentsRdsModel, CreatePaymentsStatisticsCommandResponse, CreatePaymentsStatisticsCommand>(statistics, rdsStatistics);

            if (!savedSuccessfully)
            {
                _log.Debug("Quitting operation as it failed to save the statistics");
                return null;
            }

            return new PaymentsProcessingCompletedMessage();
        }

        private async Task<bool> SaveTheStatisticsToRds<TModel, TRdsModel, TCommandResponse, TCommand>(TModel statistics, TRdsModel rdsStatistics)
            where TModel : IExternalSystemModel
            where TRdsModel : IRdsModel
            where TCommandResponse : ICommandResponse
            where TCommand : IStatisticsCommand<TModel, TRdsModel>, new()
        {
            var response = await _mediator.SendAsync((IAsyncRequest<TCommandResponse>)new TCommand
            {
                ExternalStatisticsModel = statistics,
                RdsStatisticsModel = rdsStatistics
            });

            return response.OperationSuccessful;
        }

        private async Task<EasRdsModel> RetrieveRelatedAesStatisticsFromRds()
        {
            EasRdsModel rdsStatistics = null;

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

        private async Task<CommitmentsRdsModel> RetrieveRelatedCommitmentsStatisticsFromRds(TraceWriter traceLog)
        {
            CommitmentsRdsModel rdsStatistics = null;

            _log.Debug("Gathering statistics for the equivalent commitment stats in RDS");
            traceLog.Info("Gathering statistics for the equivalent commitment stats in RDS");

            try
            {
                rdsStatistics = await _repository.RetrieveEquivalentCommitmentsStatisticsFromRds();
            }
            catch (DbException exception)
            {
                _log.Error(exception, "Failed to retrieve the equivalent commitment stats from the RDS Database");
                traceLog.Error("Failed to retrieve the equivalent commitment stats from the RDS Database", exception);
                throw;
            }
            catch (Exception ex)
            {
                traceLog.Error("General exception" + ex.Message, ex);
                throw;
            }

            return rdsStatistics;
        }

        private async Task<PaymentsRdsModel> RetrieveRelatedPaymentsStatisticsFromRds()
        {
            PaymentsRdsModel rdsStatistics = null;

            _log.Debug("Gathering statistics for the equivalent payment stats in RDS");

            try
            {
                rdsStatistics = await _repository.RetrieveEquivalentPaymentStatisticsFromRds();
            }
            catch (DbException exception)
            {
                _log.Error(exception, "Failed to retrieve the equivalent payment stats from the RDS Database");
            }

            return rdsStatistics;
        }

        private async Task<EasExternalModel> RetrieveAesStatisticsFromTheApi()
        {
            _log.Debug("Gathering statistics for the EAS area of the system");
            EasExternalModel statistics = null;

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

        private async Task<CommitmentsExternalModel> RetrieveCommitmentsStatisticsFromTheApi(TraceWriter traceLog)
        {
            _log.Debug("Gathering statistics for the Commitments area of the system");
            CommitmentsExternalModel statistics = null;

            try
            {
                statistics = await _commitmentsStatisticsHandler.Handle();
                traceLog.Info(statistics == null
                    ? "Commitments statistics returned from API was null"
                    : $"Commitments statistics returned from API, TotalCohorts: {statistics.TotalCohorts}, TotalApprenticeships: {statistics.TotalApprenticeships}, ActiveApprenticeships: {statistics.ActiveApprenticeships}");
            }
            catch (HttpRequestException httpRequestException)
            {
                _log.Error(httpRequestException, "Failed to retrieve commitment stats from the API");
                traceLog.Error("Failed to retrieve commitment stats from the API, httpRequestException", httpRequestException);
                throw;
            }
            catch (Exception ex)
            {
                traceLog.Error("Failed to retrieve commitment stats from the API", ex);
                throw;
            }

            return statistics;
        }

        private async Task<PaymentExternalModel> RetrievePaymentsStatisticsFromTheApi()
        {
            _log.Debug("Gathering statistics for the payments area of the system");
            PaymentExternalModel statistics = null;

            try
            {
                statistics = await _paymentStatisticsHandler.Handle();
            }
            catch (HttpRequestException httpRequestException)
            {
                _log.Error(httpRequestException, "Failed to retrieve payment stats from the API");
            }

            return statistics;
        }
    }
}
