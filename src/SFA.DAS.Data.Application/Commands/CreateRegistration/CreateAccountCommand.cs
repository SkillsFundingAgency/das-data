using MediatR;

namespace SFA.DAS.Data.Application.Commands.CreateRegistration
{
    public class CreateAccountCommand : IAsyncNotification
    {
        public string AccountHref { get; set; }
    }
}
