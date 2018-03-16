using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Domain.Models
{
    public class EasStatisticsModel : IExternalSystemModel
    {
        public long TotalAccounts { get; set; }
        public long TotalLegalEntities { get; set; }
        public long TotalPAYESchemes { get; set; }
        public long TotalAgreements { get; set; }
        public long TotalPayments { get; set; }
    }
}
