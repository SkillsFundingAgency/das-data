using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Commands.EasRdsStatistics
{
    public class CreateStatisticsEasCommand : IAsyncRequest<CreateStatisticsEasCommandResponse>, IAsyncRequest<CreateStatisticsEasCommandHandler>,
        IStatisticsCommand<EasStatisticsModel, RdsStatisticsForEasModel>
    {
        public EasStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForEasModel RdsStatisticsModel { get; set; }
    }
}
