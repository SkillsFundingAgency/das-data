using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.Commands.ExportPerformancePlatformStatistics
{
    public class ExportPerformancePlatformStatisticsCommandHandler : IAsyncNotificationHandler<ExportPerformancePlatformStatisticsCommand>
    {
        private readonly IEnumerable<IPerformancePlatformDataExtractor> _extractors;
        private readonly IPerformancePlatformGateway _gateway;
        private readonly IPerformancePlatformRepository _repository;

        public ExportPerformancePlatformStatisticsCommandHandler(IEnumerable<IPerformancePlatformDataExtractor> extractors, IPerformancePlatformGateway gateway, IPerformancePlatformRepository repository)
        {
            _extractors = extractors;
            _gateway = gateway;
            _repository = repository;
        }

        public async Task Handle(ExportPerformancePlatformStatisticsCommand notification)
        {
            var data = await GetData(notification);

            await _gateway.SendData(data);

            await CreateRunStatistics(notification, data);
        }

        private async Task CreateRunStatistics(ExportPerformancePlatformStatisticsCommand notification, PerformancePlatformData[] data)
        {
            var tasks = data.Select(x => _repository.CreateRunStatistics(x.DataType, notification.ExtractDateTime, x.TotalNumberOfRecords));
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
