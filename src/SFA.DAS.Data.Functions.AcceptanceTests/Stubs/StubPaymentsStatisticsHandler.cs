using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Data.Domain.Interfaces;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Functions.AcceptanceTests.Stubs
{
    public class StubPaymentsStatisticsHandler : IPaymentStatisticsHandler
    {
        public Task<PaymentStatisticsModel> Handle()
        {
            return Task.FromResult(new PaymentStatisticsModel
            {
                ProviderTotalPayments = 11
            });
        }
    }
}
