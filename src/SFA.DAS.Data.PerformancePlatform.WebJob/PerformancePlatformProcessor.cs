using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Data.Application.Commands.ExportPerformancePlatformStatistics;

namespace SFA.DAS.Data.PerformancePlatform.WebJob
{
    public class PerformancePlatformProcessor
    {
        private readonly IMediator _mediator;

        public PerformancePlatformProcessor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ExportData()
        {
            await _mediator.PublishAsync(new ExportPerformancePlatformStatisticsCommand { ExtractDateTime = DateTime.UtcNow });
        }
    }
}
