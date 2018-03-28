using System.Data.SqlClient;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.Statistics.PaymentRdsStatistics
{
    public class PaymentRdsStatisticsCommandHandler : IAsyncRequestHandler<PaymentRdsStatisticsCommand, PaymentRdsStatisticsCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public PaymentRdsStatisticsCommandHandler([Inject] IStatisticsRepository repository, [Inject] ILog log)
        {
            _repository = repository;
            _log = log;
        }
        public async Task<PaymentRdsStatisticsCommandResponse> Handle(PaymentRdsStatisticsCommand message)
        {
            var response = new PaymentRdsStatisticsCommandResponse
            {
                OperationSuccessful = true
            };

            try
            {
                await _repository.SavePaymentStatistics(message.ExternalStatisticsModel, message.RdsStatisticsModel);
            }
            catch (SqlException e)
            {
                _log.Error(e, "Failed to save the payment statistics");
                response.OperationSuccessful = false;
            }

            return response;
        }
    }
}
