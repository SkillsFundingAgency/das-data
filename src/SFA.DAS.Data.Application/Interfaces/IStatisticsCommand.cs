using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IStatisticsCommand<TExternalModel, TRdsModel> where TExternalModel : IExternalSystemModel
        where TRdsModel : IRdsModel
    {
        TExternalModel ExternalStatisticsModel { get; set; }

        TRdsModel RdsStatisticsModel { get; set; }
    }
}