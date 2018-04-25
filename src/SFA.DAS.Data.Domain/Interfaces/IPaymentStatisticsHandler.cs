using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;

namespace SFA.DAS.Data.Domain.Interfaces
{
    public interface IPaymentStatisticsHandler
    {
        Task<PaymentStatisticsModel> Handle();
    }
}
