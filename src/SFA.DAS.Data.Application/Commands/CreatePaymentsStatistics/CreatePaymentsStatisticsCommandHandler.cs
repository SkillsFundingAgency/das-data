using System.Data.SqlClient;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.CreatePaymentsStatistics
{
    public class CreatePaymentsStatisticsCommandHandler : IAsyncRequestHandler<CreatePaymentsStatisticsCommand, CreatePaymentsStatisticsCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public CreatePaymentsStatisticsCommandHandler( IStatisticsRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }
        public async Task<CreatePaymentsStatisticsCommandResponse> Handle(CreatePaymentsStatisticsCommand message)
        {
            var response = new CreatePaymentsStatisticsCommandResponse
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
