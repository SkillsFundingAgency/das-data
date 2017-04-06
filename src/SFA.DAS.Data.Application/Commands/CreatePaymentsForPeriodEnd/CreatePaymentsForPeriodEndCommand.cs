using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreatePaymentsForPeriodEnd
{
    public class CreatePaymentsForPeriodEndCommand : IAsyncNotification
    {
        public string PeriodEndId { get; set; }
    }
}
