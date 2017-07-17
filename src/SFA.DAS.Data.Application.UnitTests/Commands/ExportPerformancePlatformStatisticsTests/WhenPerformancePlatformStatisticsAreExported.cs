using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.ExportPerformancePlatformStatistics;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Commands.ExportPerformancePlatformStatisticsTests
{
    [TestFixture]
    public class WhenPerformancePlatformStatisticsAreExported
    {
        private Mock<IPerformancePlatformDataExtractor> _extractor1;
        private Mock<IPerformancePlatformDataExtractor> _extractor2;
        private Mock<IPerformancePlatformGateway> _gateway;
        private Mock<IPerformancePlatformRepository> _repository;
        private ExportPerformancePlatformStatisticsCommandHandler _commandHandler;

        [SetUp]
        public void Arrange()
        {
            _extractor1 = new Mock<IPerformancePlatformDataExtractor>();
            _extractor2 = new Mock<IPerformancePlatformDataExtractor>();
            var extractors = new List<IPerformancePlatformDataExtractor> { _extractor1.Object, _extractor2.Object };
            _gateway = new Mock<IPerformancePlatformGateway>();
            _repository = new Mock<IPerformancePlatformRepository>();
            _commandHandler = new ExportPerformancePlatformStatisticsCommandHandler(extractors, _gateway.Object, _repository.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task ThenTheStatisticsAreSentToThePerformancePlatform()
        {
            var command = new ExportPerformancePlatformStatisticsCommand { ExtractDateTime = DateTime.Now };
            var extractor1Data = new PerformancePlatformData(command.ExtractDateTime, "Type1", 10, 40);
            _extractor1.Setup(x => x.Extract(command.ExtractDateTime)).ReturnsAsync(extractor1Data);
            var extractor2Data = new PerformancePlatformData(command.ExtractDateTime, "Type2", 20, 50);
            _extractor2.Setup(x => x.Extract(command.ExtractDateTime)).ReturnsAsync(extractor2Data);

            await _commandHandler.Handle(command);

            _gateway.Verify(x => x.SendData(It.Is<IEnumerable<PerformancePlatformData>>(y => y.First() == extractor1Data && y.Last() == extractor2Data)));
        }

        [Test]
        public async Task ThenTheLastRunStatisticsAreStored()
        {
            var command = new ExportPerformancePlatformStatisticsCommand { ExtractDateTime = DateTime.Now };
            var extractor1Data = new PerformancePlatformData(command.ExtractDateTime, "Type1", 10, 40);
            _extractor1.Setup(x => x.Extract(command.ExtractDateTime)).ReturnsAsync(extractor1Data);
            var extractor2Data = new PerformancePlatformData(command.ExtractDateTime, "Type2", 20, 50);
            _extractor2.Setup(x => x.Extract(command.ExtractDateTime)).ReturnsAsync(extractor2Data);

            await _commandHandler.Handle(command);

            _repository.Verify(x => x.CreateRunStatistics(extractor1Data.Type, command.ExtractDateTime, extractor1Data.TotalNumberOfRecords));
            _repository.Verify(x => x.CreateRunStatistics(extractor2Data.Type, command.ExtractDateTime, extractor2Data.TotalNumberOfRecords));
        }
    }
}
