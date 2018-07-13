using System;

namespace SFA.DAS.Data.Domain.Models.PSRS
{
    public class ReportSubmitted
    {
        public long Id { get; set; }
        public string DasAccountId { get; set; }
        public string DasAccountName { get; set; }
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
    }

}
