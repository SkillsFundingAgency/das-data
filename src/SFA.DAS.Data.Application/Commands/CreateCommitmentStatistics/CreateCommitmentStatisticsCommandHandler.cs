using System.Data.SqlClient;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.CommitmentRdsStatistics
{
    public class CreateCommitmentStatisticsCommandHandler : IAsyncRequestHandler<CreateCommitmentStatisticsCommand, CreateCommitmentStatisticsCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public CreateCommitmentStatisticsCommandHandler( IStatisticsRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }

        public async Task<CreateCommitmentStatisticsCommandResponse> Handle(CreateCommitmentStatisticsCommand message)
        {
            var response = new CreateCommitmentStatisticsCommandResponse
            {
                OperationSuccessful = true
            };

            try
            {
                await _repository.SaveCommitmentStatistics(message.ExternalStatisticsModel, message.RdsStatisticsModel);
            }
            catch (SqlException e)
            {
                _log.Error(e, "Failed to save the commitment statistics");
                response.OperationSuccessful = false;
            }

            return response;
        }
    }
}
