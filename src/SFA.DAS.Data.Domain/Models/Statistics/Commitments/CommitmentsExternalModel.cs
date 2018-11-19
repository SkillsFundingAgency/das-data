using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Domain.Models.Statistics.Commitments
{
    public class CommitmentsExternalModel : IExternalSystemModel
    {
        public long TotalCohorts { get; set; }
        public long TotalApprenticeships { get; set; }
        public long ActiveApprenticeships { get; set; }
    }
}
