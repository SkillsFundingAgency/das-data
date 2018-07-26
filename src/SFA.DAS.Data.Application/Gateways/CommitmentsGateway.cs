using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Client.Interfaces;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Data.Application.Interfaces.Gateways;

namespace SFA.DAS.Data.Application.Gateways
{
    public class CommitmentsGateway : ICommitmentsGateway
    {
        private readonly IStatisticsApi _statisticsApi;

        public CommitmentsGateway(IStatisticsApi statisticsApi)
        {
            _statisticsApi = statisticsApi;
        }

        public async Task<ConsistencyStatistics> GetStatistics()
        {
            return await _statisticsApi.GetStatistics();
        }
    }
}
