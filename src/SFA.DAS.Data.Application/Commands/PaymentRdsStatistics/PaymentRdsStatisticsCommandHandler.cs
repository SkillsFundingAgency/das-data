using System.Data.SqlClient;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

//using SFA.DAS.Data.Application.Functions.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.PaymentRdsStatistics
{
    public class PaymentRdsStatisticsCommandHandler : IAsyncRequestHandler<PaymentRdsStatisticsCommand, PaymentRdsStatisticsCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public PaymentRdsStatisticsCommandHandler( IStatisticsRepository repository, ILog log)
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
