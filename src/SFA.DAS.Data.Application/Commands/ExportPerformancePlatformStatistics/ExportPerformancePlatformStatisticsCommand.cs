using System;
using MediatR;

namespace SFA.DAS.Data.Application.Commands.ExportPerformancePlatformStatistics
{
    public class ExportPerformancePlatformStatisticsCommand : IAsyncNotification
    {
        public DateTime ExtractDateTime { get; set; }
    }
}
