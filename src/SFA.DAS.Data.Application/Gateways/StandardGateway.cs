using System;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Data.Application.Interfaces.Gateways;

namespace SFA.DAS.Data.Application.Gateways
{
    public class StandardGateway : IStandardGateway
    {
        private readonly IStandardApiClient _standardApiClient;

        public StandardGateway(IStandardApiClient standardApiClient)
        {
            _standardApiClient = standardApiClient;
        }

        public async Task<Standard> GetStandard(string standardId)
        {
            var standard = _standardApiClient.Get(standardId);
            return standard;
        }
    }
}
