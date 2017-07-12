using System.Threading.Tasks;

namespace SFA.DAS.Data.PerformancePlatform.WebJob
{
    public interface IPerformancePlatformProcessor
    {
        Task ExportData();
    }
}
