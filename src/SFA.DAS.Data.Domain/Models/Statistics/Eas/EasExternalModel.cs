using SFA.DAS.Data.Domain.Interfaces;

namespace SFA.DAS.Data.Domain.Models.Statistics.Eas
{
    public class EasExternalModel : IExternalSystemModel
    {
        public long TotalAccounts { get; set; }
        public long TotalLegalEntities { get; set; }
        public long TotalPAYESchemes { get; set; }
        public long TotalAgreements { get; set; }
        public long TotalPayments { get; set; }
    }
}
