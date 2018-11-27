using System;
using Dapper.Contrib.Extensions;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[Das_DataLocks]")]
    public class DataLockRecord
    {
        public long Id { get; set; }
        public long DataLockEventId { get; set; }
        public DateTime ProcessDateTime { get; set; }
        public EventStatus Status { get; set; }
        public string IlrFileName { get; set; }
        public long UkPrn { get; set; }
        public long Uln { get; set; }
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long ApprenticeshipId { get; set; }
        public long EmployerAccountId { get; set; }
        public EventSource EventSource { get; set; }
        public DateTime? IlrStartDate { get; set; }
        public long? IlrStandardCode { get; set; }
        public int? IlrProgrammeType { get; set; }
        public int? IlrFrameworkCode { get; set; }
        public int? IlrPathwayCode { get; set; }
        public Decimal? IlrTrainingPrice { get; set; }
        public Decimal? IlrEndpointAssessorPrice { get; set; }
        public DateTime? IlrPriceEffectiveFromDate { get; set; }
        public DateTime? IlrPriceEffectiveToDate { get; set; }
        public bool IsLatest { get; set; }
    }
}
