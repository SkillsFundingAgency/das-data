using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Domain.Models
{
    public class PaymentStatisticsModel : IExternalSystemModel
    {
        public long ProviderTotalPayments { get; set; }
    }
}
