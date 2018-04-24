using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Domain.Models
{
    public class EasStatisticsModel
    {
        public int TotalAccounts { get; set; }
        public int TotalLegalEntities { get; set; }
        public int TotalPAYESchemes { get; set; }
        public int TotalAgreements { get; set; }
        public int TotalPayments { get; set; }
    }
}
