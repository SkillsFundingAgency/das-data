using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Commitments;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Stubs
{
    public class StubCommitmentsStatisticsHandler : ICommitmentsStatisticsHandler
    {
        public Task<CommitmentsExternalModel> Handle()
        {
            return Task.FromResult(new CommitmentsExternalModel
            {
                ActiveApprenticeships = 12,
                TotalApprenticeships = 18,
                TotalCohorts = 3
            });
        }
    }
}
