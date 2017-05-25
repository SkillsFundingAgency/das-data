using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IStandardGateway
    {
        Task<Standard> GetStandard(string standardId);
    }
}
