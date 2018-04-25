using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;

namespace SFA.DAS.Data.Application.Commands.EasRdsStatistics
{
    public class CreateStatisticsEasCommand : IAsyncRequest<CreateStatisticsEasCommandResponse>, IAsyncRequest<CreateStatisticsEasCommandHandler>,
        IStatisticsCommand<EasExternalModel, EasRdsModel>
    {
        public EasExternalModel ExternalStatisticsModel { get; set; }
        public EasRdsModel RdsStatisticsModel { get; set; }
    }
}
