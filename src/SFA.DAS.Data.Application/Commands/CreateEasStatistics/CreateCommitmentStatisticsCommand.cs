using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;

namespace SFA.DAS.Data.Application.Commands.CreateEasStatistics
{
    public class CreateEasStatisticsCommand : IAsyncRequest<CreateEasStatisticsCommandResponse>, IAsyncRequest<CreateEasStatisticsCommandHandler>,
        IStatisticsCommand<EasExternalModel, EasRdsModel>
    {
        public EasExternalModel ExternalStatisticsModel { get; set; }
        public EasRdsModel RdsStatisticsModel { get; set; }
    }
}
