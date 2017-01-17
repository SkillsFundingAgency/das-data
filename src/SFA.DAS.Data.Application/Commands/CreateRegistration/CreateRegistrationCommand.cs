using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommand : IAsyncNotification
    {
        public string DasAccountId { get; set; }
    }
}
