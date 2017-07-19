using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.Commands.ExportPerformancePlatformStatistics
{
    public class ExportPerformancePlatformStatisticsCommandHandler : IAsyncNotificationHandler<ExportPerformancePlatformStatisticsCommand>
    {
        private readonly IEnumerable<IPerformancePlatformDataExtractor> _extractors;
        private readonly IPerformancePlatformGateway _gateway;
        private readonly IPerformancePlatformRepository _repository;
        private readonly ILog _logger;

        public ExportPerformancePlatformStatisticsCommandHandler(IEnumerable<IPerformancePlatformDataExtractor> extractors, IPerformancePlatformGateway gateway, IPerformancePlatformRepository repository, ILog logger)
        {
            _extractors = extractors;
            _gateway = gateway;
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(ExportPerformancePlatformStatisticsCommand notification)
        {
            _logger.Info("Getting data for publishing to the performance platform.");
            var data = await GetData(notification);

            _logger.Info("Sending data to the performance platform.");
            await _gateway.SendData(data);

            _logger.Info("Creating performance platform run statistics");
            await CreateRunStatistics(notification, data);
        }

        private async Task CreateRunStatistics(ExportPerformancePlatformStatisticsCommand notification, PerformancePlatformData[] data)
        {
            var tasks = data.Select(x => _repository.CreateRunStatistics(x.Type, notification.ExtractDateTime, x.TotalNumberOfRecords));
            await Task.WhenAll(tasks);
        }

        private async Task<PerformancePlatformData[]> GetData(ExportPerformancePlatformStatisticsCommand notification)
        {
            var tasks = _extractors.Select(x => x.Extract(notification.ExtractDateTime));
            var data = await Task.WhenAll(tasks);
            return data;
        }
    }
}
