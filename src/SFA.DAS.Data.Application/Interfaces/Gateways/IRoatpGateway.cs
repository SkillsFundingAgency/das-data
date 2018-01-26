using System.Collections.Generic;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IRoatpGateway
    {
        Roatp.Api.Types.Provider GetProvider(string ukPrn);
    }
}
