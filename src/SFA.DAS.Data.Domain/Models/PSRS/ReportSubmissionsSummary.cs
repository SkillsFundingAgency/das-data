namespace SFA.DAS.Data.Domain.Models.PSRS
{
    public class ReportSubmissionsSummary
    {
        public long Id { get; set; }
        public int SubmittedTotals { get; set; }
        public int InProcessTotals { get; set; }
        public int ViewedTotals { get; set; }
        public int ReportingPeriod { get; set; }
    }
}
