using System;
using Dapper.Contrib.Extensions;

namespace SFA.DAS.Data.AcceptanceTests.Data.DTOs
{
    [Table("[Data_Load].[DAS_Employer_PayeSchemes]")]
    public class EmployerPayeSchemesRecord
    {
        public long Id { get; set; }
        public string DasAccountId { get; set; }
        public string Ref { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool IsLatest { get; set; }
            
    }
}
