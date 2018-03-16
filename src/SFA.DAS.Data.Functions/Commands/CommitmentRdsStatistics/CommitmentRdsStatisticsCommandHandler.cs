using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Functions.Commands.EasRdsStatistics;

namespace SFA.DAS.Data.Functions.Commands.CommitmentRdsStatistics
{
    public class CommitmentRdsStatisticsCommandHandler : IAsyncRequestHandler<CommitmentRdsStatisticsCommand, CommitmentRdsStatisticsCommandResponse>
    {
        public Task<CommitmentRdsStatisticsCommandResponse> Handle(CommitmentRdsStatisticsCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
