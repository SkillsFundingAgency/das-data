using System.Data.SqlClient;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Functions.Commands.CommitmentRdsStatistics
{
    public class CommitmentRdsStatisticsCommandHandler : IAsyncRequestHandler<CommitmentRdsStatisticsCommand, CommitmentRdsStatisticsCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public CommitmentRdsStatisticsCommandHandler([Inject] IStatisticsRepository repository, [Inject] ILog log)
        {
            _repository = repository;
            _log = log;
        }

        public async Task<CommitmentRdsStatisticsCommandResponse> Handle(CommitmentRdsStatisticsCommand message)
        {
            var response = new CommitmentRdsStatisticsCommandResponse
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
