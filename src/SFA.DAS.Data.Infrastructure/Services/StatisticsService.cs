using System;
using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.CommitmentRdsStatistics;
using SFA.DAS.Data.Application.Commands.EasRdsStatistics;
using SFA.DAS.Data.Application.Commands.PaymentRdsStatistics;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Application.Messages;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Statistics.Services
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

            var savedSuccessfully = await SaveTheStatisticsToRds<EasStatisticsModel, RdsStatisticsForEasModel, CreateStatisticsEasCommandResponse, CreateStatisticsEasCommand>(statistics, rdsStatistics);

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

            var savedSuccessfully = await SaveTheStatisticsToRds<PaymentStatisticsModel, RdsStatisticsForPaymentsModel, PaymentRdsStatisticsCommandResponse, PaymentRdsStatisticsCommand>(statistics, rdsStatistics);

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

        private async Task<RdsStatisticsForPaymentsModel> RetrieveRelatedPaymentsStatisticsFromRds()
        {
            RdsStatisticsForPaymentsModel rdsStatistics = null;

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

        private async Task<PaymentStatisticsModel> RetrievePaymentsStatisticsFromTheApi()
        {
            _log.Debug("Gathering statistics for the payments area of the system");
            PaymentStatisticsModel statistics = null;

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
