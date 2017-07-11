using System;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces
{
    public interface IPerformancePlaformDataExtractor
    {
        Task<PerformancePlanData> Extract(DateTime extractDateTime);
    }
}
