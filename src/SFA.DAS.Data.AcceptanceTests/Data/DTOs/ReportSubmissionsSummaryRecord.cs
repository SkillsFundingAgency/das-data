using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_PublicSector_Summary]")]
    public class ReportSubmissionsSummaryRecord
    {
        public long Id { get; set; }
        public int SubmittedTotals { get; set; }
        public int InProcessTotals { get; set; }
        public int ViewedTotals { get; set; }
        public string ReportingPeriod { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsLatest { get; set; }
        public int Total { get; set; }
    }
}
