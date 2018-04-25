using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Domain.Models
{
    public class CommitmentsStatisticsModel : IExternalSystemModel
    {
        public long TotalCohorts { get; set; }
        public long TotalApprenticeships { get; set; }
        public long ActiveApprenticeships { get; set; }
    }
}
