using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Domain.Models.Statistics.Payments
{
    public class PaymentExternalModel : IExternalSystemModel
    {
        public long ProviderTotalPayments { get; set; }
    }
}
