using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Statistics.Commands;
using SFA.DAS.Data.Functions.Statistics.Commands.CommitmentRdsStatistics;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CommitmentRdsStatistics
{
    [TestFixture]
    public class WhenTheHandleMethodIsInvoked :
        FunctionsCommandHandlerBase<CommitmentsStatisticsModel, RdsStatisticsForCommitmentsModel, CommitmentRdsStatisticsCommand>
    {
        private CommitmentRdsStatisticsCommandHandler _handler;

        protected override void InitialiseHandler()
        {
            _handler = new CommitmentRdsStatisticsCommandHandler(Repository.Object, Log.Object);
        }

        protected override Expression<Func<IStatisticsRepository, Task>> RepositoryExpression()
        {
            return o =>
                o.SaveCommitmentStatistics(It.IsAny<CommitmentsStatisticsModel>(), It.IsAny<RdsStatisticsForCommitmentsModel>());
        }

        protected override CommitmentRdsStatisticsCommand StatisticsCommand()
        {
            return new CommitmentRdsStatisticsCommand            {
                ExternalStatisticsModel = new CommitmentsStatisticsModel(),
                RdsStatisticsModel = new RdsStatisticsForCommitmentsModel()
            };
        }

        protected override async Task<ICommandResponse> InvokeTheHandler(CommitmentRdsStatisticsCommand command)
        {
            return await _handler.Handle(command);
        }
    }
}
