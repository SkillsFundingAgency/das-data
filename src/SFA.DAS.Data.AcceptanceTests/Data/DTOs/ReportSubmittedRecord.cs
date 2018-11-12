using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_PublicSector_Reports]")]

    public class ReportSubmittedRecord
    {
        public long Id { get; set; }
        public string DasAccountId { get; set; }
        public string OrganisationName { get; set; }
        public string ReportingPeriod { get; set; }
        public int FigureA { get; set; }
        public int FigureB { get; set; }
        public decimal FigureE { get; set; }
        public int FigureC { get; set; }
        public int FigureD { get; set; }
        public decimal FigureF { get; set; }
        public int FigureG { get; set; }
        public int FigureH { get; set; }
        public decimal FigureI { get; set; }
        public int? FullTimeEquivalent { get; set; }
        public string OutlineActions { get; set; }
        public int OutlineActionsWordCount { get; set; }
        public string Challenges { get; set; }
        public int ChallengesWordCount { get; set; }
        public string TargetPlans { get; set; }
        public int TargetPlansWordCount { get; set; }
        public string AnythingElse { get; set; }
        public int AnythingElseWordCount { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string SubmittedName { get; set; }
        public string SubmittedEmail { get; set; }
        public bool IsLatest { get; set; }
    }
}
