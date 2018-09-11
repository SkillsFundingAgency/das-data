using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[Das_DataLocks]")]
    public class DataLockRecord
    {
        public long Id { get; set; }
        public long DataLockId { get; set; }
        public DateTime ProcessDateTime { get; set; }
        public string IlrFileName { get; set; }
        public long UkPrn { get; set; }
        public long Uln { get; set; }
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long ApprenticeshipId { get; set; }
        public long EmployerAccountId { get; set; }
        public int EventSource { get; set; }
        public DateTime? IlrStartDate { get; set; }
        public long? IlrStandardCode { get; set; }
        public int? IlrProgrammeType { get; set; }
        public int? IlrFrameworkCode { get; set; }
        public int? IlrPathwayCode { get; set; }
        public Decimal? IlrTrainingPrice { get; set; }
        public Decimal? IlrEndpointAssessorPrice { get; set; }
        public DateTime? IlrPriceEffectiveFromDate { get; set; }
        public DateTime? IlrPriceEffectiveToDate { get; set; }

        public string ErrorCode { get; set; }
        public string SystemDescription { get; set; }

        public string ApprenticeshipVersion { get; set; }
        public string CalenderPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public bool IsPayable { get; set; }
        public int TransactionType { get; set; }

        public string Version { get; set; }
        public DateTime StartDate { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
        public Decimal NegotiatedPrice { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
