using System;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Repositories
{
    public interface IPerformancePlatformRepository
    {
        Task<long> GetNumberOfRecordsFromLastRun(string dataType);
        Task CreateRunStatistics(string dataType, DateTime runDateTime, long numberOfRecords);
    }
}
