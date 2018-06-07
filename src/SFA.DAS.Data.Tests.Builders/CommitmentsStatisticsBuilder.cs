using SFA.DAS.Commitments.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class CommitmentsStatisticsBuilder
    {
        public ConsistencyStatistics Build()
        {
            return new ConsistencyStatistics
            {
                TotalApprenticeships = 100, TotalCohorts = 12, ActiveApprenticeships = 76
            };
        }
    }
}
