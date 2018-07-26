using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface ICommitmentsGateway
    {
        Task<ConsistencyStatistics> GetStatistics();
    }
}
