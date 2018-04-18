using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands.EasRdsStatistics;

namespace SFA.DAS.Data.Functions.Commands.CommitmentRdsStatistics
{
    public class CommitmentRdsStatisticsCommand : IAsyncRequest<CommitmentRdsStatisticsCommandResponse>, IAsyncRequest<CommitmentRdsStatisticsCommandHandler>, IStatisticsCommand<CommitmentsStatisticsModel, RdsStatisticsForCommitmentsModel>
    {
        public CommitmentsStatisticsModel ExternalStatisticsModel { get; set; }
        public RdsStatisticsForCommitmentsModel RdsStatisticsModel { get; set; }
    }
}
