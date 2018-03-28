using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Commands.Statistics
{
    public interface IStatisticsCommand<TExternalModel, TRdsModel> where TExternalModel : IExternalSystemModel
        where TRdsModel : IRdsModel        
    {
        TExternalModel ExternalStatisticsModel { get; set; }

        TRdsModel RdsStatisticsModel { get; set; }
    }
}