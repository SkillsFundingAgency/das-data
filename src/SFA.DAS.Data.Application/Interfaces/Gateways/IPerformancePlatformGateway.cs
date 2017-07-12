using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IPerformancePlatformGateway
    {
        Task SendData(IEnumerable<PerformancePlatformData> data);
    }
}
