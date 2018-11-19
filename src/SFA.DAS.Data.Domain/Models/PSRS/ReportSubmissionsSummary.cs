using System;

namespace SFA.DAS.Data.Domain.Models.PSRS
{
    public class ReportSubmissionsSummary
    {
        public DateTime ToDate { get; set; }
        public int SubmittedTotals { get; set; }
        public int InProcessTotals { get; set; }
        public int ViewedTotals { get; set; }
        public int Total { get; set; }
        public string ReportingPeriod { get; set; }
    }
}
