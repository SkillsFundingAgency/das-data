using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Statistics.Commands;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Commands
{
    public abstract class FunctionsCommandHandlerBase<T, TRdsModel, TStatsCommand> 
        where T : IExternalSystemModel, new()
        where TRdsModel : IRdsModel
        where TStatsCommand : IStatisticsCommand<T, TRdsModel>
    {
        protected Mock<IStatisticsRepository> Repository;
        protected Mock<ILog> Log;

        [SetUp]
        public void Setup()
        {
            Log = new Mock<ILog>();
            Repository = new Mock<IStatisticsRepository>();
            
            InitialiseHandler();
        }

        protected abstract void InitialiseHandler();

        [Test]
        public async Task ThenTheDetailsAreSaved()
        {
            var response = await InvokeTheHandler(StatisticsCommand());

            Assert.AreEqual(true, response.OperationSuccessful);
            Repository.Verify(RepositoryExpression(), Times.Once);
        }

        [Test]
        public async Task IfTheRepositoryReturnsAnErrorTheOperationDoesntSucceed()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

            Repository.Setup(RepositoryExpression()).Throws(exception);

            var response = await InvokeTheHandler(StatisticsCommand());

            Assert.AreEqual(false, response.OperationSuccessful);
        }

        protected abstract Expression<Func<IStatisticsRepository, Task>> RepositoryExpression();

        protected abstract TStatsCommand StatisticsCommand();

        protected abstract Task<ICommandResponse> InvokeTheHandler(TStatsCommand command);
    }
}
