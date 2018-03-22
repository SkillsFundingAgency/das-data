using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands;
using SFA.DAS.Data.Functions.Commands.EasRdsStatistics;

namespace SFA.DAS.Data.Application.UnitTests.Commands.EasRdsStatisticsTests
{
    [TestFixture]
    public class WhenTheHandleMethodIsInvoked : 
        FunctionsCommandHandlerBase<EasStatisticsModel, RdsStatisticsForEasModel, EasRdsStatisticsCommand>
    {
        private EasRdsStatisticsCommandHandler _handler;
       
        protected override void InitialiseHandler()
        {
            _handler = new EasRdsStatisticsCommandHandler(Repository.Object, Log.Object);
        }

        protected override Expression<Func<IStatisticsRepository, Task>> RepositoryExpression()
        {
            return o =>
                o.SaveEasStatistics(It.IsAny<EasStatisticsModel>(), It.IsAny<RdsStatisticsForEasModel>());
        }

        protected override EasRdsStatisticsCommand StatisticsCommand()
        {
            return new EasRdsStatisticsCommand
            {
                ExternalStatisticsModel = new EasStatisticsModel(),
                RdsStatisticsModel = new RdsStatisticsForEasModel()
            };
        }

        protected override async Task<ICommandResponse> InvokeTheHandler(EasRdsStatisticsCommand command)
        {
            return await _handler.Handle(command);
        }
    }
}
