using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Eas;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Stubs
{
    public class StubEasStatisticsHandler : IEasStatisticsHandler
    {
        public Task<EasExternalModel> Handle()
        {
            return Task.FromResult(new EasExternalModel
            {
                TotalPayments = 3,
                TotalAccounts = 1,
                TotalAgreements = 2,
                TotalPAYESchemes = 1,
                TotalLegalEntities = 1
            });
        }
    }
}
