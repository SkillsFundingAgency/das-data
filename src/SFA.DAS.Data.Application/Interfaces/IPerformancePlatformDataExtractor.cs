using System;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IPerformancePlatformDataExtractor
    {
        Task<PerformancePlatformData> Extract(DateTime extractDateTime);
    }
}
