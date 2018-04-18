using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Functions.Commands.EasRdsStatistics;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Commands.EasRdsStatisticsTests
{
    [TestFixture]
    public class WhenTheHandleMethodIsInvoked
    {
        private EasRdsStatisticsCommandHandler _handler;
        private Mock<IStatisticsRepository> _repository;
        private Mock<ILog> _log;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _repository = new Mock<IStatisticsRepository>();
            _handler = new EasRdsStatisticsCommandHandler(_repository.Object, _log.Object);
        }

        [Test]
        public async Task ThenTheDetailsAreSaved()
        {
            var response = await InvokeTheHandler();

            Assert.AreEqual(true, response.OperationSuccessful);
            _repository.Verify(o => o.SaveEasStatistics(It.IsAny<EasStatisticsModel>(), It.IsAny<RdsStatisticsForEasModel>()), Times.Once);
        }

        private async Task<EasRdsStatisticsCommandResponse> InvokeTheHandler()
        {
            var response = await _handler.Handle(new EasRdsStatisticsCommand
            {
                ExternalStatisticsModel = new EasStatisticsModel(),
                RdsStatisticsModel = new RdsStatisticsForEasModel()
            });

            return response;
        }

        [Test]
        public async Task IfTheRepositoryReturnsAnErrorTheOperationDoesntSucceed()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

            _repository.Setup(o =>
                    o.SaveEasStatistics(It.IsAny<EasStatisticsModel>(), It.IsAny<RdsStatisticsForEasModel>()))
                .Throws(exception);

            var response = await InvokeTheHandler();

            Assert.AreEqual(false, response.OperationSuccessful);
        }
    }
}
