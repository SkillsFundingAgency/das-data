using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Ioc;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.EasRdsStatistics
{
    public class EasRdsStatisticsCommandHandler : IAsyncRequestHandler<EasRdsStatisticsCommand, EasRdsStatisticsCommandResponse>
    {
        private readonly IStatisticsRepository _repository;
        private readonly ILog _log;

        public EasRdsStatisticsCommandHandler([Inject] IStatisticsRepository repository, [Inject] ILog log)
        {
            _repository = repository;
            _log = log;
        }

        public async Task<EasRdsStatisticsCommandResponse> Handle(EasRdsStatisticsCommand message)
        {
            var response = new EasRdsStatisticsCommandResponse
            {
                OperationSuccessful = true
            };

            try
            {
               await _repository.SaveEasStatistics(message.EasStatisticsModel, message.RdsStatisticsForEasModel);
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
