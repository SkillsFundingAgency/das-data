using System.Data.SqlClient;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.EasRdsStatistics
{
    public class CreateStatisticsEasCommandHandler : IAsyncRequestHandler<CreateStatisticsEasCommand, CreateStatisticsEasCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public CreateStatisticsEasCommandHandler( IStatisticsRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }

        public async Task<CreateStatisticsEasCommandResponse> Handle(CreateStatisticsEasCommand message)
        {
            var response = new CreateStatisticsEasCommandResponse
            {
                OperationSuccessful = true
            };

            try
            {
                await _repository.SaveEasStatistics(message.ExternalStatisticsModel, message.RdsStatisticsModel);
            }
            catch (SqlException e)
            {
                _log.Error(e, "Failed to save the Eas statistics");
                response.OperationSuccessful = false;
            }

            return response;
        }
    }
}
