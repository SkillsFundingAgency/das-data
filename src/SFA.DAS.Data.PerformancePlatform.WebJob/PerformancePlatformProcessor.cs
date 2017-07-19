using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.ExportPerformancePlatformStatistics;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.PerformancePlatform.WebJob
{
    public class PerformancePlatformProcessor
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;

        public PerformancePlatformProcessor(IMediator mediator, ILog logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task ExportData()
        {
            try
            {
                await _mediator.PublishAsync(new ExportPerformancePlatformStatisticsCommand { ExtractDateTime = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred whilst exporting performance platform data");
            }
        }
    }
}
