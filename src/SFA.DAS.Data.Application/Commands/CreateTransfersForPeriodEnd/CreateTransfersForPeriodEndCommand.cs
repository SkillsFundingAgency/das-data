using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateTransfersForPeriodEnd
{
    public class CreateTransfersForPeriodEndCommand : IAsyncNotification
    {
        public string PeriodEndId { get; set; }
    }
}
