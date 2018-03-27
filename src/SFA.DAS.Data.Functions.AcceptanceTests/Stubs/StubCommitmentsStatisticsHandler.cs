using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Stubs
{
    public class StubCommitmentsStatisticsHandler : ICommitmentsStatisticsHandler
    {
        public Task<CommitmentsStatisticsModel> Handle()
        {
            return Task.FromResult(new CommitmentsStatisticsModel
            {
                ActiveApprenticeships = 12,
                TotalApprenticeships = 18,
                TotalCohorts = 3
            });
        }
    }
}
