using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_Employer_LegalEntities]")]
    public class EmployerLegalEntitiesRecord
    {
        public long Id { get; set; }
        public string DasAccountId { get; set; }
        public long DasLegalEntityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Source { get; set; }
        public DateTime? InceptionDate { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool IsLatest { get; set; }
    }
}
